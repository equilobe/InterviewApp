using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.ViewModels
{
    public class EditViewModel
    {
        public Question Question { get; set; }
        public string ViewPath { get; set; }
        public bool IsNew { get; set; }
    }
}