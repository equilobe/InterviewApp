using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.ViewModels
{
    public class WelcomeModel
    {
        public TestDelivery.DAL.Respondent Respondent { get; set; }
        public Test NextTest { get; set; }
        public TimeSpan TotalDuration { get; set; }
    }
}