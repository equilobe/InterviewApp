using System;
namespace TestDelivery.ServiceLayer.Models
{
    public interface ITestTemplateExtraInfo
    {
        bool CanEdit { get; }
        TestDelivery.DAL.TestTemplate Template { get; set; }
        int TestCount { get; set; }
    }
}
