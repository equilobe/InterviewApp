using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels.Templates
{
    public class EditQuestionViewModel
    {
        public TestTemplate Template { get; set; }
        public Question Question { get; set; }
        public string ViewPath { get; set; }
    }
}