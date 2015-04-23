using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestDelivery.Common.Extensions;
using TestDelivery.DAL.Questions.Metadata;

namespace TestDelivery.DAL.Questions
{
    [MetadataType(typeof(QuestionMetadata))]
    public partial class Question : IQuestion
    {
        protected abstract string SerializeExtendedProperties();

        private bool _extenedPropertiesUsed = false;

        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]
        [DataMemberAttribute()]
        internal global::System.String ExtendedPropertiesXml
        {
            get
            {
                if (_extenedPropertiesUsed)
                {
                    _extendedPropertiesXml = SerializeExtendedProperties();
                }
                return _extendedPropertiesXml;
            }
            set
            {
                _extendedPropertiesXml = value;
            }
        }
        private string _extendedPropertiesXml;

        protected T GetExtendedProperties<T>(ref T obj, Func<string, T> deserializeFunc) where T : new()
        {
            if (!_extenedPropertiesUsed)
            {
                if (_extendedPropertiesXml != null)
                    obj = deserializeFunc(_extendedPropertiesXml);
                else
                    obj = new T();
                _extenedPropertiesUsed = true;
            }
            return obj;
        }

        protected T GetExtendedProperties<T>(ref T obj) where T : new()
        {
            return GetExtendedProperties<T>(ref obj, DeserializeExtendedProperties<T>);
        }

        protected T DeserializeExtendedProperties<T>(string xml) where T : new()
        {
            if (xml == null)
                return new T();

            return GetSimpleSerializer<T>().DeserializeFromString<T>(xml);
        }

        protected string SerializeExtendedProperties<T>(T obj)
        {
            return GetSimpleSerializer<T>().SerializeToString(obj);
        }

        public static XmlSerializer GetSimpleSerializer<T>()
        {
            return new XmlSerializer(typeof(T));
        }

        protected void SetExtendedProperties<T>(ref T obj, T value)
        {
            if (object.ReferenceEquals(obj, value))
                return;

            obj = value;
            _extenedPropertiesUsed = true;
        }
        
        public bool IsNewQuestion
        {
            get
            {
                return Id == Guid.Empty;
            }
        }


        static object _syncRoot = new object();
        static Type[] _questionTypes;
        public static Type[] QuestionTypes
        {
            get
            {
                lock (_syncRoot)
                {
                    if (_questionTypes == null)
                    {
                        _questionTypes = typeof(Question).GetDerivedTypes().ToArray();
                    }
                    return _questionTypes;
                }
            }
        }

        public virtual void BeforeSave()
        {
        }

        public TimeSpan Duration
        {
            get
            {
                return TimeSpan.FromSeconds(this.TimeLimit);
            }
        }
    }
}
