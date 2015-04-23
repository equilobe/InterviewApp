using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;
using TestDelivery.ServiceLayer.Lists;


namespace TestDelivery.ServiceLayer
{
    public class QuestionService : IQuestionService
    {

        public List<Question> ListQuestions()
        {
            using (var context = new QuestionsDao())
            {
                return (from q in context.Questions
                        select q).ToList();
            }
        }

        public void SaveQuestion(Question question)
        {
            question.BeforeSave();
            using (var context = new QuestionsDao())
            {
                context.Questions.Attach(question);
                context.ObjectStateManager.ChangeObjectState(question, System.Data.EntityState.Modified);
                context.SaveChanges();
            }
        }


        public Question GetQuestionById(Guid id)
        {
            using (var context = new QuestionsDao())
            {
                return (from q in context.Questions
                        where q.Id == id
                        select q).FirstOrDefault();
            }
        }


        public void AddQuestion(Question question)
        {
            question.Id = Guid.NewGuid();
            question.BeforeSave();
            using (var context = new QuestionsDao())
            {
                context.Questions.AddObject(question);
                context.SaveChanges();
            }
        }


        public void DeleteQuestion(Guid Id)
        {
            using (var context = new QuestionsDao())
            {
                Question question = (from q in context.Questions
                                     where q.Id == Id
                                     select q).FirstOrDefault();
                context.DeleteObject(question);
                context.SaveChanges();
            }
        }


        public Page<Question> GetQuestions(PageInfo pageInfo)
        {
            using (var context = new QuestionsDao())
            {
                var questions = context.Questions.OrderBy(q => q.Tags);

                var count = questions.Count();
                var skipCount = (pageInfo.Page - 1) * pageInfo.PageSize;

                var pagedQuestions = questions.Skip(skipCount)
                                              .Take(pageInfo.PageSize);

                var startPostion = 1 + skipCount;
                var hasNextPage = count > pageInfo.Page * pageInfo.PageSize;
                return new Page<Question>()
                {
                    CurrentPage = pageInfo.Page,
                    Items = pagedQuestions.ToArray(),
                    HasPreviousPage = pageInfo.Page > 1,
                    HasNextPage = hasNextPage,
                    StartItemPosition = startPostion,
                    EndItemPosition =  hasNextPage ? startPostion + pageInfo.PageSize - 1 : count,
                    TotalItemsCount = count
                };
            }
        }
    }

    public interface IQuestionService
    {
        List<Question> ListQuestions();
        void SaveQuestion(Question question);
        Question GetQuestionById(Guid id);
        void AddQuestion(Question question);
        void DeleteQuestion(Guid id);
        Page<Question> GetQuestions(PageInfo pageInfo);
    }
}
