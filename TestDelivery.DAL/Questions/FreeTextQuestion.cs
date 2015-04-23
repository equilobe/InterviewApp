using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestDelivery.DAL.Questions.Metadata;

namespace TestDelivery.DAL.Questions
{
    [MetadataType(typeof(FreeTextQuestionMetadata))]    
    public partial class FreeTextQuestion : Question, IFreeTextQuestion
    {
        public List<string> CorrectAnswers { get { return Options.CorrectAnswers; } set { Options.CorrectAnswers = value; } }
        public List<string> WrongAnswers { get { return Options.WrongAnswers; } set { Options.WrongAnswers = value; } }
        public string EvaluatorNotes { get { return Options.EvaluatorNotes; } set { Options.EvaluatorNotes = value; } }

        protected override string SerializeExtendedProperties()
        {
            return SerializeExtendedProperties<FreeTextQuestionOptions>(Options);
        }

        FreeTextQuestionOptions _options;
        private FreeTextQuestionOptions Options
        {
            get
            {
                return GetExtendedProperties<FreeTextQuestionOptions>(ref _options);
            }
            set
            {
                SetExtendedProperties<FreeTextQuestionOptions>(ref _options, value);
            }
        }

        public class FreeTextQuestionOptions
        {
            public List<string> CorrectAnswers { get; set; }
            public List<string> WrongAnswers { get; set; }
            public string EvaluatorNotes { get; set; }
        }

        public bool IsCorrectAnswer(FreeTextAnswer answer)
        {
            if (string.IsNullOrWhiteSpace(answer.Text))
                return false;

            return CorrectAnswers
                       .Select(a => a.ToLower().Trim())
                       .Contains(answer.Text.ToLower().Trim());
        }

        public bool IsWrongAnswer(FreeTextAnswer answer)
        {
            if (string.IsNullOrWhiteSpace(answer.Text))
                return true;

            return WrongAnswers
                       .Select(a => a.ToLower().Trim())
                       .Contains(answer.Text.ToLower().Trim());
        }

        public void AutoEvaluateAnswer(FreeTextAnswer answer)
        {
            if (IsCorrectAnswer(answer))
                answer.Evaluate(Score);
            else if (IsWrongAnswer(answer))
                answer.Evaluate(0);
        }
    }
}
