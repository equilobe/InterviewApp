using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;
using TestDelivery.ServiceLayer;
using TestDelivery.ServiceLayer.Lists;
using TestDelivery.UI.Helpers;
using TestDelivery.UI.ViewModels;
using TestDelivery.UI.ViewModels.Questions;

namespace TestDelivery.UI.Controllers
{   
    public class QuestionsController : AdminController
    {
        public IQuestionService QuestionService { get; set; }
        public IQuestionViewService QuestionViewService { get; set; }

        public ActionResult Index()
        {
            return View(new IndexViewModel
            {
                NewQuestionControllerNames = QuestionViewService.GetQuestionControllerNames()
            });
        }

        public ActionResult DeleteQuestion(string id)
        {
            QuestionService.DeleteQuestion(Guid.Parse(id));
            return RedirectToAction("Index");
        }

        public ActionResult Edit(Guid id)
        {
            Question question = QuestionService.GetQuestionById(id);

            return View(new EditViewModel()
            {
                Question = question,
                ViewPath = QuestionViewService.GetEditPartialViewPath(question)
            });
        }
      
        public ActionResult GetTemplateQuestions(PageInfo pageInfo)
        {
            var model = QuestionService.GetQuestions(pageInfo);
            return View(model);
        }

        public ActionResult GetQuestions(PageInfo pageInfo)
        {
            var model = QuestionService.GetQuestions(pageInfo);
            return View(model);
        }
    }
}
