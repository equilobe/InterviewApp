using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.UI.ViewModels;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer;
using TestDelivery.DAL.Questions;
using TestDelivery.UI.Helpers;
using TestDelivery.UI.ViewModels.Evaluate;

namespace TestDelivery.UI.Controllers
{

    public class EvaluateTestController : AdminController
    {
        public ITestService TestService { get; set; }
        public ITemplateService TemplateService { get; set; }
        public IQuestionViewService QuestionViewService { get; set; }
        public IQuestionService QuestionService { get; set; }

        [HttpGet]
        public ActionResult Index()
        {
            List<Test> model = TestService.GetCompletedTests();
            return View(model);
        }

        public ActionResult Evaluate(Guid id)
        {
            var test = TestService.GetTestById(id);
            return View(test);
        }

        public ActionResult EvaluateTest(Guid id)
        {
            var test = TestService.GetTestById(id);

            EvaluateTestVM model = new EvaluateTestVM
            {
                Test = test,
                Answers = test.TestTemplate.Questions.Select(q => new EvaluateAnswerVM
                {
                    Test = test,
                    Question = q,
                    Answer = test.GetAnswer(q.Id),
                    ViewPath = QuestionViewService.GetEvaluatePartialViewPath(q)
                })
            };

            return View(model);
        }

        public ActionResult EvaluateQuestion(Guid id, Guid testId)
        {
            Question q = QuestionService.GetQuestionById(id);
            Test test = TestService.GetTestById(testId);
            Answer a = test.Answers.Where(ans => ans.QuestionId == id).FirstOrDefault();
            EvaluateAnswerVM qa = new EvaluateAnswerVM();
            qa.Question = q;
            qa.Answer = a;
            return View(QuestionViewService.GetEvaluatePartialViewPath(q), qa);
        }



    }
}
