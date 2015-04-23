using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.BL.AnswerResolvers
{
    [AnswerResolver(typeof(MultipleChoiceQuestion))]
    public class MultipleChoiceQuestionAnswerResolver : IAnswerResolver
    {
        public void FillAnswer(Question q, Answer a, NameValueCollection formValues)
        {
            var question = (MultipleChoiceQuestion)q;
            var answer = (MultipleChoiceAnswer)a;
            TrySetGivenAnswers(formValues, answer);
            question.AutoEvaluateAnswer(answer);
        }

        private static void TrySetGivenAnswers(NameValueCollection formValues, MultipleChoiceAnswer answer)
        {
            try
            {
                answer.GivenAnswerIds = formValues[FreeTextQuestionAnswerResolver.ANSWER_FIELD_NAME]
                                                     .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Select(str => new Guid(str))
                                                     .ToArray();
            }
            catch { }
        }

        public Answer GetAnswer(Question q)
        {
            return new MultipleChoiceAnswer() { QuestionId = q.Id };
        }
    }
}
