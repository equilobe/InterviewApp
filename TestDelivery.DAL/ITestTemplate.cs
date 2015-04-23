using System;
namespace TestDelivery.DAL
{
    public interface ITestTemplate
    {
        string EndInstructions { get; set; }
        Guid Id { get; set; }
        bool IsActive { get; set; }
        bool IsQuestionTimeLimitEnforced { get; set; }
        bool IsRandomOrder { get; set; }
        string Name { get; set; }
        int Priority { get; set; }
        System.Collections.Generic.List<TestDelivery.DAL.Questions.Question> Questions { get; set; }
        string StartInstructions { get; set; }
        int TimeLimit { get; set; }
    }
}
