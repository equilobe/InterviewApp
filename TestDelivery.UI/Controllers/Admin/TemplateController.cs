using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;
using TestDelivery.ServiceLayer;
using TestDelivery.ServiceLayer.Models;
using TestDelivery.UI.Helpers;
using TestDelivery.UI.ViewModels;
using TestDelivery.UI.ViewModels.Templates;

namespace TestDelivery.UI.Controllers
{
    public class TemplateController : AdminController
    {
        public ITemplateService TemplateService { get; set; }
        public IQuestionService QuestionService { get; set; }
        public IQuestionViewService QuestionViewService { get; set; }

        public ActionResult Index()
        {
            List<TestTemplate> list = TemplateService.ListTemplates();
            return View(list);
        }

        public ActionResult Delete(string id)
        {
            TemplateService.DeleteTemplate(Guid.Parse(id));
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            return View(new EditTemplateViewModel
            {
                Info = TemplateService.GetExtraInfo(id),
                NewQuestionControllerNames = QuestionViewService.GetQuestionControllerNames()
            });
        }

        [HttpPost]
        public ActionResult New(string name)
        {
            var template = new TestTemplate()
            {
                Name = name
            };

            TemplateService.AddTemplate(template);

            return RedirectToAction("Edit", new { id = template.Id });
        }

        [HttpPost]
        public ActionResult Save(TestTemplate template, string QuestionIds)
        {
            template.Questions = GetTemplateQuestionsFromIds(template, QuestionIds);

            if (template.IsNew)
            {
                TemplateService.AddTemplate(template);
            }
            else
            {
                TemplateService.SaveTemplate(template);
            }

            return RedirectToAction("Edit", new { id = template.Id });
        }

        private List<Question> GetTemplateQuestionsFromIds(TestTemplate template, string QuestionIds)
        {
            TestTemplate oldTemplate = null;
            if (!template.IsNew)
                oldTemplate = TemplateService.GetTemplateById(template.Id);

            return QuestionIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(str => str.StartsWith("@") ?
                                            oldTemplate.GetQuestion(new Guid(str.Substring(1))) :
                                            QuestionService.GetQuestionById(new Guid(str)))
                            .ToList();
        }

        public ActionResult DeleteTests(string id)
        {

            return null;
        }

        [HttpGet]
        public ActionResult EditQuestion(Guid id, Guid questionId)
        {
            var template = TemplateService.GetTemplateById(id);
            var question = template.GetQuestion(questionId);

            return View(new EditQuestionViewModel
            {
                Question = question,
                Template = template,
                ViewPath = QuestionViewService.GetEditPartialViewPath(question)
            });
        }

        [HttpGet]
        public ActionResult Questions(Guid id)
        {
            var template = TemplateService.GetTemplateById(id);
            return View(template.Questions);
        }

        public ActionResult Question(Guid id, Guid questionId)
        {
            var template = TemplateService.GetTemplateById(id);
            var question = template.GetQuestion(questionId);
            return View(question);
        }

        public ActionResult Copy(Guid id)
        {
            var toCopy = TemplateService.GetTemplateById(id);
            if (toCopy == null)
                return null;

            var copy = TemplateService.CopyTemplate(toCopy);

            return RedirectToAction("Edit", new { id = copy.Id });
        }
    }
}
