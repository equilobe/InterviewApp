using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TestDelivery.Common.Extensions;

namespace TestDelivery.DAL.Questions
{
    public partial class MultipleChoiceQuestion : Question
    {
        public List<PossibleAnswer> Answers { get { return Options.Answers; } set { Options.Answers = value; } }

        protected override string SerializeExtendedProperties()
        {
            return SerializeExtendedProperties<MultipleChoiceQuestionOptions>(Options);
        }

        MultipleChoiceQuestionOptions _options;
        private MultipleChoiceQuestionOptions Options
        {
            get
            {
                return GetExtendedProperties<MultipleChoiceQuestionOptions>(ref _options);
            }
            set
            {
                SetExtendedProperties<MultipleChoiceQuestionOptions>(ref _options, value);
            }
        }

        public class MultipleChoiceQuestionOptions
        {
            public List<PossibleAnswer> Answers { get; set; }
        }

        public void AutoEvaluateAnswer(MultipleChoiceAnswer answer)
        {
            var correctAnswerIds = this.Answers
                                      .Where(a => a.IsCorrect)
                                      .Select(a => a.Id);
            
            if (correctAnswerIds.Count() == 0)
                answer.Evaluate(Score);
            else if (answer.GivenAnswerIds == null)
                answer.Evaluate(0);
            else if (correctAnswerIds.Count() != answer.GivenAnswerIds.Count())
                answer.Evaluate(0);
            else if (correctAnswerIds.Except(answer.GivenAnswerIds).Any())
                answer.Evaluate(0);
            else
                answer.Evaluate(Score);
        }

        public override void BeforeSave()
        {
            if (Answers != null)
            {
                foreach (var answer in Answers.Where(a => a.Id == Guid.Empty))
                {
                    answer.Id = Guid.NewGuid();
                }
            }

            base.BeforeSave();
        }

        public bool HasMultipleAnswers
        {
            get
            {
                return Answers != null && Answers.Count(a => a.IsCorrect) > 1;
            }
        }
    }
}
