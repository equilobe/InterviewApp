using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDelivery.BL.AnswerResolvers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AnswerResolverAttribute : Attribute
    {
        public AnswerResolverAttribute(Type questionType)
        {
            this.QuestionType = questionType;
        }

        public Type QuestionType { get; private set; }
    }
}
