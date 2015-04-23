using System;
namespace TestDelivery.DAL.Questions
{
    public interface IQuestion
    {
        string Content { get; set; }
        Guid Id { get; set; }
        int Score { get; set; }
        string Summary { get; set; }
        string Tags { get; set; }
        int TimeLimit { get; set; }
    }
}
