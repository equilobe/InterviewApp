using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer;
using TestDelivery.UI.ViewModels;

namespace TestDelivery.UI.Controllers
{
    public class InterviewController : ApplicationController
    {
        public IRespondentService RespondentService { get; set; }
        public ITestService TestService { get; set; }

        public ActionResult Welcome(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return null;

            var respondent = RespondentService.GetRespondentBySecretCode(code.Trim());
            if (respondent == null)
                return null;

            return View(new WelcomeModel
            {
                Respondent = respondent,
                NextTest = TestService.GetNextTestForRespondent(respondent.Id),
                TotalDuration = GetTotalTestDurationForRespondent(respondent.Id)
            });
        }

        private TimeSpan GetTotalTestDurationForRespondent(Guid respondentId)
        {
            return TestService.GetByRespondentId(respondentId)
                         .Where(t => !t.IsComplete)
                         .Select(t => t.TestTemplate.MaxDuration)
                         .Aggregate(new TimeSpan(), (total, duration) => total + duration);
        }

    }
}
