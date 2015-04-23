using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.ViewModels
{
    public class RespondentTestsList
    {
        public int Score { get; set; }
        public int UncompletedTests { get; set; }
        public int TestsInProgress { get; set; }
        public int UnevaluatedTests { get; set; }
        public Respondent Respondent { get; set; }
        public List<Test> Tests { get; set; }
    }
}