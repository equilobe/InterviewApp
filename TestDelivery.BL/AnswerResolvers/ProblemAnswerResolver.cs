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
    [AnswerResolver(typeof(Problem))]
    public class ProblemAnswerResolver : IAnswerResolver
    {
        public void FillAnswer(Question q, Answer a, NameValueCollection formValues)
        {
            var problem = (Problem)q;
            var problemAnswer = (ProblemAnswer)a;

            if (problem.AnswerSections == null)
            {
                problemAnswer.SectionAnswers = null;
                return;
            }

            problemAnswer.SectionAnswers = new string[problem.AnswerSections.Count];

            for (int i = 0; i < problem.AnswerSections.Count; i++)
            {
                try
                {
                    problemAnswer.SectionAnswers[i] = formValues[FreeTextQuestionAnswerResolver.ANSWER_FIELD_NAME + "[" + i + "]"];
                }
                catch { }
            }
        }

        public Answer GetAnswer(Question q)
        {
            return new ProblemAnswer() { QuestionId = q.Id };
        }
    }

}
