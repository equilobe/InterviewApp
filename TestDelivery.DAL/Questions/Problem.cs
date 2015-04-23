using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL.Questions.Metadata;

namespace TestDelivery.DAL.Questions
{
    [MetadataType(typeof(ProblemMetadata))]
    public partial class Problem : Question, IProblem
    {
        public List<AnswerSection> AnswerSections { get { return Options.AnswerSections; } set { Options.AnswerSections = value; } }
        
        public string EvaluatorNotes { get { return Options.EvaluatorNotes; } set { Options.EvaluatorNotes = value; } }

        protected override string SerializeExtendedProperties()
        {
            return SerializeExtendedProperties<ProblemOptions>(Options);
        }

        ProblemOptions _options;
        private ProblemOptions Options
        {
            get
            {
                return GetExtendedProperties<ProblemOptions>(ref _options);
            }
            set
            {
                SetExtendedProperties<ProblemOptions>(ref _options, value);
            }
        }

        public class ProblemOptions
        {
            public List<AnswerSection> AnswerSections { get; set; }
            public string EvaluatorNotes { get; set; }
        }

        public class AnswerSection
        {
            public string Name { get; set; }
        }
    }
}
