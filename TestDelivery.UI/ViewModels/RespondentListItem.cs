using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.ViewModels
{
    public class RespondentListItem
    {
        public Respondent Respondent { get; set; }
        public ExtraInfo ExtraInfo { get; set; }
    }
}