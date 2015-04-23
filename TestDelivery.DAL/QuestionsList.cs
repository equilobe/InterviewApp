using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestDelivery.DAL.Questions;

namespace TestDelivery.DAL
{
    public class QuestionsList
    {
        [XmlElement("Question")]
        public List<Question> Questions { get; set; }

        public QuestionsList()
        {
            Questions = new List<Question>();
        }
    }
}
