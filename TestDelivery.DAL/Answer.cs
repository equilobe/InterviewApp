using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestDelivery.DAL
{
    public class Answer
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        //public string Text { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsExpired { get; set; }
        public int Score { get; set; }
        public bool IsEvaluated { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (!EndTime.HasValue || !StartTime.HasValue)
                    return TimeSpan.MinValue;

                return EndTime.Value - StartTime.Value;
            }
        }

        public int GetTimeLimitPercent(int timeLimit)
        {
            if (Duration == TimeSpan.MinValue)
                return 0;

            if (timeLimit == 0)
                return -1;

            return (int)Duration.TotalSeconds * 100 / timeLimit;
        }

        public void Evaluate(int score)
        {
            Score = score;
            IsEvaluated = true;
        }

        public void Expire()
        {
            IsExpired = true;
            StartTime = StartTime.HasValue ? StartTime.Value : DateTime.Now;
            EndTime = EndTime.HasValue ? EndTime.Value : DateTime.Now;
            Evaluate(0);
        }

        public void Start()
        {
            StartTime = DateTime.Now;
        }

        public void End()
        {
            EndTime = DateTime.Now;
        }
    }

    public class FreeTextAnswer : Answer
    {
        public string Text { get; set; }
    }

    public class MultipleChoiceAnswer : Answer
    {
        public Guid[] GivenAnswerIds { get; set; }
    }

    public class ProblemAnswer : Answer
    {
        public string[] SectionAnswers { get; set; }
    }
}