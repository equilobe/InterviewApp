using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.BL.AnswerResolvers
{
    public interface IAnswerResolver
    {
        Answer GetAnswer(Question q);
        void FillAnswer(Question q, Answer a, NameValueCollection formValues);
    }
}
