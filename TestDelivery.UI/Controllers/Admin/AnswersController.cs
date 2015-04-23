using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer;

namespace TestDelivery.UI.Controllers
{

    public class AnswersController : AdminController
    {
        public IEvaluateService EvaluateService { get; set; }

        public ActionResult Correct(Guid testId, Guid questionId)
        {
            try
            {
                EvaluateService.MarkCorrect(testId, questionId);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult Incorrect(Guid testId, Guid questionId)
        {
            try
            {
                EvaluateService.MarkIncorrect(testId, questionId);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        public ActionResult Score(Guid testId, Guid questionId, int score)
        {
            try
            {
            
                EvaluateService.Evaluate(testId, questionId, score);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }


    }
}
