using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDelivery.DAL;

namespace TestDelivery.UI.Models
{
    public class TestModel
    {
        public TestTemplate Template { get; set; }
        public Test Test { get; set; }

        public TestModel()
        {
            TestTemplate Template = new TestTemplate();
            Test Test = new Test();
        }
    }
}