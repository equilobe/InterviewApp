using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer;
using System.Data.Entity;
using System.Data.Objects;
using TestDelivery.UI.Models;
using TestDelivery.UI.ViewModels;
using TestDelivery.DAL.Questions;
using TestDelivery.UI.Helpers;

namespace TestDelivery.UI.Controllers
{

    public class TestController :ApplicationController
    {
        public ITestService TestService { get; set; }
        public IAnswerService AnswerService { get; set; }
        public IQuestionViewService QuestionViewService { get; set; }

        [HttpGet]
        public ActionResult Take(string code)
        {
            Test test = TestService.GetTestBySecretCode(code);
            if (test == null)
                return null;
            return View(test);
        }

        [HttpGet]
        public ActionResult Welcome(string code)
        {
            Test test = TestService.GetTestBySecretCode(code);
            if (test == null)
                return null;

            if (!test.HasStarted)
                return View(test);

            Question question = TestService.GetCurrentQuestion(test);

            return HandleQuestion(test, question);
        }

        private TestQuestionModel GetQuestionModel(Test test, Question question)
        {
            var questionStartTime = test.GetAnswer(question.Id).StartTime;
            var now = DateTime.Now;
            var questionTimePassed = now - questionStartTime.Value;
            var testTimePassed = now - test.DateStarted.Value;

            return new TestQuestionModel
            {
                Question = question,
                TimePassed = Convert.ToInt32(questionTimePassed.TotalSeconds),
                TestTimePassed = Convert.ToInt32(testTimePassed.TotalSeconds),
                TestTimeLimit = test.TestTemplate.TimeLimit,
                QuestionNumber = test.Answers.Count,
                QuestionCount = test.TestTemplate.Questions.Count,
                TestName = test.TestTemplate.Name,
                QuestionViewPath = QuestionViewService.GetTestPartialViewPath(question),
            };
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Next(string code, FormCollection formValues)
        {
            Test test = TestService.GetTestBySecretCode(code);
            if (test == null)
                return null;

            Question question = TestService.GetNextQuestion(test, formValues);
            return HandleQuestion(test, question);
        }

        private ActionResult HandleQuestion(Test test, Question question)
        {
            if (question == null)
            {
                return View("Finish", new FinishTest
                {
                    CurrentTest = test,
                    NextTest = GetNextTest(test)
                });
            }

            return View("Question", GetQuestionModel(test, question));
        }

        private Test GetNextTest(Test test)
        {
            return TestService.GetNextTestForRespondent(test.Respondent.Id);
        }

    }


}
