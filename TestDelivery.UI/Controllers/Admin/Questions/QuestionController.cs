using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL.Questions;
using TestDelivery.ServiceLayer;
using TestDelivery.ServiceLayer.Models;
using TestDelivery.UI.Helpers;
using TestDelivery.UI.ViewModels;

namespace TestDelivery.UI.Controllers
{
    public abstract class QuestionController<T> : AdminController
        where T : Question, new()
    {
        public IQuestionService QuestionService { get; set; }
        public ITemplateService TemplateService { get; set; }
        public IQuestionViewService QuestionViewService { get; set; }

        public virtual ActionResult New()
        {
            var question = new T();
            return View("~/Views/Questions/Edit.cshtml", new EditViewModel
            {
                Question = question,
                ViewPath = QuestionViewService.GetEditPartialViewPath(question),
                IsNew = true
            });
        }

        [HttpPost]
        public virtual ActionResult Save(T problem)
        {
            return SaveQuestion(problem);
        }

        [HttpGet]
        public virtual ActionResult NewTemplateQuestion(Guid templateId)
        {
            return NewTemplateQuestion(templateId, new T());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveTemplateQuestion(Guid templateId, [Bind(Prefix = "editQuestion")] T editedQuestion)
        {
            return TrySaveTemplateQuestion(templateId, editedQuestion);
        }

        protected ActionResult SaveQuestion(Question q)
        {
            if (q.IsNewQuestion)
                QuestionService.AddQuestion(q);
            else
                QuestionService.SaveQuestion(q);

            return RedirectToAction("Index", "Questions");
        }

        protected ActionResult TrySaveTemplateQuestion(Guid templateId, T editedQuestion)
        {
            try
            {
                DoSaveTemlateQuestion(templateId, editedQuestion);
                return Json(true);
            }
            catch
            {
                return Json(false);
            }
        }

        protected void DoSaveTemlateQuestion(Guid templateId, T editedQuestion)
        {
            var templateInfo = TemplateService.GetExtraInfo(templateId);
            var template = templateInfo.Template;
            var oldQuestion = (T)template.GetQuestion(editedQuestion.Id);
            UpdateQuestion(templateInfo, editedQuestion, oldQuestion);
            template.SaveQuestion(editedQuestion);
            TemplateService.SaveTemplate(template);
        }

        protected virtual void UpdateQuestion(TestTemplateExtraInfo templateInfo, T editedQuestion, T oldQuestion)
        {
            if (!templateInfo.CanEdit)
            {
                editedQuestion.Score = oldQuestion.Score;
            }
        }

        protected ActionResult NewTemplateQuestion(Guid templateId, Question question)
        {
            var template = TemplateService.GetTemplateById(templateId);

            question.Id = Guid.NewGuid();
            question.Summary = "New question";

            template.Questions.Add(question);
            TemplateService.SaveTemplate(template);

            return Json(new { questionId = question.Id, templateId = template.Id }, JsonRequestBehavior.AllowGet);
        }
    }
}
