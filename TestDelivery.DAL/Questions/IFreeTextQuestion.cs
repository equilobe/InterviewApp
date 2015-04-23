using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.DAL.Questions
{
    public interface IFreeTextQuestion : IQuestion
    {
        string EvaluatorNotes { get; set; }
    }
}
