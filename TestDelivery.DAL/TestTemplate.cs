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
using System.ComponentModel.DataAnnotations;
using TestDelivery.DAL.Metadata;

namespace TestDelivery.DAL
{
    [MetadataType(typeof(TestTemplateMetadata))]
    public partial class TestTemplate : TestDelivery.DAL.ITestTemplate
    {
        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        internal string QuestionsXML
        {
            get
            {
                return SerializationHelpers.GetXml(ref _questionsXML, _questionsUsed, _questions, GetQuestionsListSerializer);
            }
            set
            {
                _questionsXML = value;
            }
        }
        private string _questionsXML;

        private bool _questionsUsed = false;

        public List<Question> Questions
        {
            get
            {
                return SerializationHelpers.GetObject<List<Question>>(ref _questions, ref _questionsUsed, _questionsXML, GetQuestionsListSerializer);
            }
            set
            {
                SerializationHelpers.SetObject<List<Question>>(ref _questions, ref _questionsUsed, value);
            }
        }
        private List<Question> _questions = null;

        private static XmlSerializer GetQuestionsListSerializer()
        {
            var serializedType = typeof(List<Question>);
            var attributeOverrides = new XmlAttributeOverrides();
            var ignoreAttribute = new XmlAttributes()
            {
                XmlIgnore = true
            };
            attributeOverrides.Add(typeof(EntityObject), "EntityKey", ignoreAttribute);
            var knownTypes = Question.QuestionTypes;

            return new XmlSerializer(serializedType, attributeOverrides, knownTypes,
                                     new XmlRootAttribute(), string.Empty);
        }

        public bool IsNew { get { return Id == Guid.Empty; } }

        public Question GetQuestion(Guid id)
        {
            return Questions.Where(q => q.Id == id).First();
        }

        public void SaveQuestion(Question question)
        {
            question.BeforeSave();
            ReplaceQuestion(question);
        }

        private void ReplaceQuestion(Question question)
        {
            var oldQuestion = Questions.Where(q => q.Id == question.Id).First();
            var index = Questions.IndexOf(oldQuestion);
            Questions.Remove(oldQuestion);
            Questions.Insert(index, question);
        }

        public TimeSpan MaxDuration
        {
            get
            {
                var testLimit = TimeLimit > 0 ? TimeSpan.FromSeconds(TimeLimit) : TimeSpan.MaxValue;

                if (Questions == null)
                    return testLimit;

                if (!Questions.All(q => q.TimeLimit > 0))
                    return testLimit;

                var addedQuestionSeconds = Questions.Aggregate<Question, int>(0, (limit, q) => limit + q.TimeLimit);

                var questionsLimit = TimeSpan.FromSeconds(addedQuestionSeconds);

                return testLimit < questionsLimit ? testLimit : questionsLimit;
            }
        }
    }
}
