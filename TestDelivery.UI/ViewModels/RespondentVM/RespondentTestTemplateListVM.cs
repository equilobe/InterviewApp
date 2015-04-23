using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.ViewModels.RespondentVM
{
    public class RespondentTestTemplateListVM
    {
        public List<TestTemplate> Templates { get; set; }
        public Guid RespondentId { get; set; }
    }
}