using System;
namespace TestDelivery.ServiceLayer
{
    public interface IEvaluateService
    {
        void Evaluate(Guid testId, Guid questionId, int score);
        void MarkCorrect(Guid testId, Guid questionId);
        void MarkIncorrect(Guid testId, Guid questionId);
    }
}
