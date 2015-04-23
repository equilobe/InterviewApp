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
    [AnswerResolver(typeof(FreeTextQuestion))]
    public class FreeTextQuestionAnswerResolver : IAnswerResolver
    {
        public const string ANSWER_FIELD_NAME = "answer";

        public void FillAnswer(Question q, Answer a, NameValueCollection formValues)
        {
            var answer = (FreeTextAnswer)a;
            var question = (FreeTextQuestion)q;
            answer.Text = formValues[ANSWER_FIELD_NAME];
            question.AutoEvaluateAnswer(answer);
        }

        public Answer GetAnswer(Question q)
        {
            return new FreeTextAnswer() { QuestionId = q.Id };
        }
    }
}
