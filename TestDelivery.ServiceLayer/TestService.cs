using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL;
using System.Data.Entity;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using TestDelivery.DAL.Questions;
using System.Collections.Specialized;
using TestDelivery.Common;

namespace TestDelivery.ServiceLayer
{
    public class TestService : ITestService
    {
        public IAnswerService AnswerService { get; set; }

        public Test GetTestBySecretCode(string code)
        {
            using (var context = new TestDeliveryEntities())
            {
                return context.Test
                            .Include(tp => tp.TestTemplate)
                            .Include(rp => rp.Respondent)
                            .Where(t => t.SecretCode.ToLower() == code.ToLower())
                            .FirstOrDefault();
            }
        }


        public void SaveTest(Test test)
        {
            test.Evaluate();
            using (var context = new TestDeliveryEntities())
            {
                context.Test.Attach(test);
                context.ObjectStateManager.ChangeObjectState(test, System.Data.EntityState.Modified);
                context.SaveChanges();
            }
        }


        public Question GetNextQuestion(Test test, NameValueCollection formValues)
        {
            EnforceTestTimeLimit(test);

            if (test.IsComplete)
                return null;

            SaveAnswer(test, formValues);

            return StartNextQuestion(test);
        }

        private Question StartNextQuestion(Test test)
        {
            var question = (from q in test.TestTemplate.Questions
                            where IsQuestionAnswered(test, q.Id) == false
                            select q).FirstOrDefault();

            if (question != null)
            {
                if (!IsQuestionStarted(test, question.Id))
                    StartAnswer(test, question);
            }
            else
            {
                test.DateCompleted = DateTime.Now;
                SaveTest(test);
            }

            return question;
        }

        public Question GetCurrentQuestion(Test test)
        {

            EnforceTestTimeLimit(test);

            if (test.IsComplete)
                return null;

            var startedQuestion = GetCurrentStartedQuestion(test);
            if (startedQuestion != null)
                return startedQuestion;

            return StartNextQuestion(test);
        }

        private Question GetCurrentStartedQuestion(Test test)
        {
            var answer = GetStartedQuestionAnswer(test);
            if (answer == null)
                return null;
            var question = GetQuestionFromAnswer(test, answer);

            if (QuestionTimeLimitExceeded(test, question, answer))
            {
                answer.Expire();
                SaveTest(test);
                return null;
            }

            return question;
        }

        private static Answer GetStartedQuestionAnswer(Test test)
        {
            return test.Answers.Where(a => !a.EndTime.HasValue).FirstOrDefault();
        }

        private void EnforceTestTimeLimit(Test test)
        {
            if (!test.HasStarted)
            {
                test.DateStarted = DateTime.Now;
                SaveTest(test);
            }

            if (test.TestTemplate.TimeLimit <= 0)
                return;

            DateTime now = DateTime.Now;
            var diff = now - test.DateStarted.Value;
            if (test.TestTemplate.TimeLimit < diff.TotalSeconds)
            {
                CloseAllQuestions(test);
                test.DateCompleted = now;
                SaveTest(test);
            }
        }

        private void CloseAllQuestions(Test test)
        {
            var ans = from answer in test.Answers
                      where answer.EndTime == null
                      select answer;

            foreach (var answer in ans)
                answer.Expire();

            foreach (var question in test.TestTemplate.Questions)
                if (!IsQuestionStarted(test, question.Id))
                {
                    var answer = AnswerService.GetAnswer(question);
                    answer.Expire();
                    test.Answers.Add(answer);
                }
        }

        private Question GetQuestionFromAnswer(Test test, Answer answer)
        {
            return test.TestTemplate.Questions.Where(q => q.Id == answer.QuestionId).First();
        }

        public const int TIME_LIMIT_EXTENSION_SECONDS = 10;

        private bool QuestionTimeLimitExceeded(Test test, Question question, Answer answer)
        {
            if (question.TimeLimit <= 0) return false;
            var diff = DateTime.Now - answer.StartTime.Value;

            if (diff.TotalSeconds > question.TimeLimit)
                return true;

            return false;
        }

        private bool QuestionTimeLimitAllowsSaving(Test test, Question question, Answer answer)
        {
            if (question.TimeLimit <= 0)
                return true;

            var diff = DateTime.Now - answer.StartTime.Value;

            if (diff.TotalSeconds < question.TimeLimit + TIME_LIMIT_EXTENSION_SECONDS)
                return true;

            return false;
        }

        private void StartAnswer(Test test, Question question)
        {
            var answer = AnswerService.GetAnswer(question);
            answer.Start();

            test.Answers.Add(answer);
            SaveTest(test);
        }
        private bool IsQuestionStarted(Test test, Guid id)
        {
            var question = (from a in test.Answers
                            where a.QuestionId == id && a.EndTime == null
                            select a).FirstOrDefault();
            if (question == null) return false;
            return true;
        }
        private bool IsQuestionAnswered(Test test, Guid questionId)
        {
            return (from a in test.Answers
                    where a.QuestionId == questionId
                    select a)
                   .Any();
        }

        public void SaveAnswer(Test test, NameValueCollection formValues)
        {
            var answer = GetStartedQuestionAnswer(test);
            if (answer == null)
                return;

            var question = GetQuestionFromAnswer(test, answer);
            if (question == null)
                return;

            if (QuestionTimeLimitAllowsSaving(test, question, answer))
            {
                AnswerService.FillAnswer(question, answer, formValues);
                answer.End();
            }
            else
            {
                answer.Expire();
            }

            SaveTest(test);
        }

        public List<Test> GetCompletedTests()
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from t in context.Test.Include(x => x.Respondent).Include(t => t.TestTemplate)
                        where t.DateCompleted != null
                        select t).ToList();
            }
        }


        public Test GetTestById(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                return context.Test
                                   .Include(x => x.TestTemplate)
                                   .Include(t => t.Respondent)
                              .Where(t => t.Id == id)
                              .FirstOrDefault();
            }
        }

        public void AddTest(Test test)
        {
            test.Id = Guid.NewGuid();
            test.SecretCode = SecretCodeGenerator.GetNewCode();

            using (var context = new TestDeliveryEntities())
            {
                context.Test.AddObject(test);
                context.SaveChanges();
            }
        }


        public void DeleteTestsByRespondentId(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                var tests = from test in context.Test
                            where test.RespondentId == id
                            select test;

                foreach (var test in tests)
                    DeleteTest(test.Id);

            }
        }


        public void DeleteTest(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                Test test = (from t in context.Test
                             where t.Id == id
                             select t).FirstOrDefault();
                context.DeleteObject(test);
                context.SaveChanges();

            }
        }

        public List<Test> GetByRespondentId(Guid id)
        {
            using (var context = new TestDeliveryEntities())
            {
                return (from test in context.Test.Include(t => t.TestTemplate)
                        where test.RespondentId == id
                        select test).ToList();
            }
        }


        public Test GetNextTestForRespondent(Guid respondentId)
        {
            using (var context = new TestDeliveryEntities())
            {
                return context.Test.Include(t => t.TestTemplate)
                    .Where(t => t.RespondentId == respondentId)
                    .Where(t => t.DateCompleted == null)
                    .OrderByDescending(t => t.TestTemplate.Priority)
                    .FirstOrDefault();
            }
        }
    }

    public interface ITestService
    {
        Test GetTestBySecretCode(string code);
        void SaveTest(Test test);
        Question GetCurrentQuestion(Test test);
        Question GetNextQuestion(Test test, NameValueCollection formValues);
        List<Test> GetCompletedTests();
        Test GetTestById(Guid id);
        void AddTest(Test test);
        void DeleteTestsByRespondentId(Guid id);
        void DeleteTest(Guid id);
        List<Test> GetByRespondentId(Guid id);
        Test GetNextTestForRespondent(Guid respondentId);
    }
}
