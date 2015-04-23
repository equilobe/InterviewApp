using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.EntityClient;
using System.Data.Entity;
using System.Data.EntityModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Xml.Serialization;
using System.IO;
using TestDelivery.Common.Extensions;
using TestDelivery.DAL.Questions;

namespace TestDelivery.DAL
{
    public partial class Test
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        internal global::System.String AnswersXML
        {
            get
            {
                return SerializationHelpers.GetXml(ref _answersXML, _answersUsed, _answers, GetAnswersListSerializer);
            }
            set
            {
                _answersXML = value;
            }
        }
        private string _answersXML;

        private bool _answersUsed = false;


        public List<Answer> Answers
        {
            get
            {
                return SerializationHelpers.GetObject<List<Answer>>(ref _answers, ref _answersUsed, _answersXML, GetAnswersListSerializer);
            }
            set
            {
                SerializationHelpers.SetObject<List<Answer>>(ref _answers, ref _answersUsed, value);
            }
        }
        private List<Answer> _answers = null;

        public bool HasStarted { get { return DateStarted != null; } }
        public bool IsComplete { get { return DateCompleted != null; } }

        private static XmlSerializer GetAnswersListSerializer()
        {
            var serializedType = typeof(List<Answer>);
            var attributeOverrides = new XmlAttributeOverrides();
            var ignoreAttribute = new XmlAttributes()
            {
                XmlIgnore = true
            };
            attributeOverrides.Add(typeof(EntityObject), "EntityKey", ignoreAttribute);
            var knownTypes = typeof(Answer).GetDerivedTypes().ToArray();

            return new XmlSerializer(serializedType, attributeOverrides, knownTypes,
                                     new XmlRootAttribute(), string.Empty);
        }

        public Answer GetAnswer(Question question)
        {
            return GetAnswer(question.Id);
        }

        public Answer GetAnswer(Guid questionId)
        {
            return Answers.Where(a => a.QuestionId == questionId)
                          .First();
        }

        public void Evaluate()
        {
            var actualScore = GetActualScore();
            var maxScore = GetMaxScore();
            this.ScorePercent = actualScore * 100 / maxScore;
            if (this.AreAllQuestionsEvaluated)
                this.DateEvaluated = DateTime.Now;
        }

        private int GetActualScore()
        {
            return this.Answers
                       .Where(a => a.IsEvaluated)
                       .Aggregate<Answer, int>(0, (score, a) => score + a.Score);
        }

        private int GetMaxScore()
        {
            return this.TestTemplate.Questions
                       .Aggregate<Question, int>(0, (score, q) => score + q.Score);
        }

        public bool IsEvaluated
        {
            get
            {
                return DateEvaluated.HasValue;
            }
        }

        public bool AreAllQuestionsEvaluated
        {
            get
            {
                return this.TestTemplate.Questions.Count == this.Answers.Where(a => a.IsEvaluated).Count();
            }
        }

        public TimeSpan Duration
        {
            get
            {
                if (!DateStarted.HasValue || !DateCompleted.HasValue)
                    return TimeSpan.MinValue;

                return DateCompleted.Value - DateStarted.Value;
            }
        }
    }
}
