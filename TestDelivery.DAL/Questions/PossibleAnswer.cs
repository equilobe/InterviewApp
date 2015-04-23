using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.DAL.Questions
{
    public class PossibleAnswer
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
