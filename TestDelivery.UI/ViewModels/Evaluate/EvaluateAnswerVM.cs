using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels.Evaluate
{
    public class EvaluateAnswerVM
    {
        public Question Question { get; set; }
        public Answer Answer { get; set; }
        public Test Test { get; set; }
        public string ViewPath { get; set; }
    }
}