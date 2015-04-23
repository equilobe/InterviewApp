using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.ServiceLayer.Models;

namespace TestDelivery.UI.ViewModels.Templates
{
    public class EditTemplateViewModel : ITestTemplateExtraInfo
    {
        public TestTemplateExtraInfo Info { get; set; }
        public IEnumerable<string> NewQuestionControllerNames { get; set; }

        public bool CanEdit
        {
            get { return Info.CanEdit; }
        }

        public DAL.TestTemplate Template
        {
            get
            {
                return Info.Template;
            }
            set
            {
                Info.Template = value;
            }
        }

        public int TestCount
        {
            get
            {
                return Info.TestCount;
            }
            set
            {
                Info.TestCount = value;
            }
        }
    }
}