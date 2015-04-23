using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestDelivery.UI.ViewModels
{
    public class ExtraInfo
    {
        public int Score { get; set; }
        public int TestsInProgress { get; set; }
        public int UnevaluatedTests { get; set; }
        public int UnstartedTests { get; set; }
    }
}