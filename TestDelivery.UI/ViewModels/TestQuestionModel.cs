using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels
{
    public class TestQuestionModel
    {
        public Question Question {get;set;}
        public int TimePassed { get; set; }
        public int TestTimePassed { get; set; }
        public int QuestionNumber { get; set; }
        public int QuestionCount { get; set; }
        public int TestTimeLimit { get; set; }
        public string QuestionViewPath { get; set; }
        public string TestName { get; set; }
    }
}