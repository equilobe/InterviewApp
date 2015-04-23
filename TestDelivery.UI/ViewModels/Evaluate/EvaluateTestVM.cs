using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels.Evaluate
{
    public class EvaluateTestVM
    {
        public Test Test { get; set; }
        public IEnumerable<EvaluateAnswerVM> Answers { get; set; }
    }
}