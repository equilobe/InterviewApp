using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.ServiceLayer
{
    public class EvaluateService : TestDelivery.ServiceLayer.IEvaluateService 
    {
        public ITestService TestService { get; set; }

        public void MarkCorrect(Guid testId, Guid questionId)
        {
            Evaluate(testId, questionId, int.MaxValue);
        }


        public void MarkIncorrect(Guid testId, Guid questionId)
        {
            Evaluate(testId, questionId, 0);
        }

        public void Evaluate(Guid testId, Guid questionId, int score)
        {
            var test = TestService.GetTestById(testId);
            var question = test.TestTemplate.GetQuestion(questionId);
            var answer = test.GetAnswer(questionId);

            if (score > question.Score)
                score = question.Score;

            if (score < 0)
                score = 0;

            answer.Score = score;
            answer.IsEvaluated = true;

            TestService.SaveTest(test);
        }
    }
}
