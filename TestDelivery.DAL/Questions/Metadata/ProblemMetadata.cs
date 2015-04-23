using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestDelivery.DAL.Questions.Metadata
{
    public class ProblemMetadata : QuestionMetadata, IProblem
    {
        [AllowHtml]
        [DataType(DataType.MultilineText)]       
        public string EvaluatorNotes { get; set; }
    }
}
