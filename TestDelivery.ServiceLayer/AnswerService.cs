using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.BL.AnswerResolvers;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.ServiceLayer
{
    public class AnswerService : IAnswerService
    {
        public void FillAnswer(Question q, Answer a, NameValueCollection formValues)
        {
            var resolver = GetAnswerResolver(q);
            if (resolver == null)
                return;
            resolver.FillAnswer(q, a, formValues);
        }

        public Answer GetAnswer(Question q)
        {
            var resolver = GetAnswerResolver(q);
            
            Answer answer = null;
            
            if (resolver == null)
                answer = new Answer();
            else
                answer = resolver.GetAnswer(q);

            answer.Id = Guid.NewGuid();
            answer.QuestionId = q.Id;

            return answer;
        }

        private static IAnswerResolver GetAnswerResolver(Question q)
        {
            Type questionType = q.GetType();
            var resolverType = typeof(IAnswerResolver).Assembly.GetTypes()
                                            .Where(t =>
                                            {
                                                var attr = t.GetCustomAttributes(typeof(AnswerResolverAttribute), false)
                                                            .Cast<AnswerResolverAttribute>()
                                                            .FirstOrDefault();
                                                return attr != null && attr.QuestionType == questionType;
                                            }).FirstOrDefault();
            if (resolverType == null)
                return null;
            return (IAnswerResolver) Activator.CreateInstance(resolverType);
        }
    }

    public interface IAnswerService
    {
        void FillAnswer(Question q, Answer a, System.Collections.Specialized.NameValueCollection formValues);
        Answer GetAnswer(Question q);

    }
}
