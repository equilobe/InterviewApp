using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL;

namespace TestDelivery.ServiceLayer.Models
{
    public class TestTemplateExtraInfo : ITestTemplateExtraInfo
    {
        public TestTemplate Template { get; set; }
        public int TestCount { get; set; }

        public bool CanEdit { get { return TestCount == 0; } }
    }
}
