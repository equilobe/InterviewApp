using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels.Questions
{
    public class IndexViewModel
    {
        public IEnumerable<string> NewQuestionControllerNames { get; set; }
    }
}