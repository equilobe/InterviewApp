using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.ViewModels
{
    public class FinishTest
    {
        public Test NextTest { get; set; }
        public Test CurrentTest { get; set; }
    }
}