using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDelivery.DAL;
using TestDelivery.ServiceLayer;
using TestDelivery.UI.ViewModels;
using TestDelivery.UI.ViewModels.RespondentVM;

namespace TestDelivery.UI.Controllers
{
    public class RespondentController : AdminController
    {
        public IRespondentService RespondentService { get; set; }
        public ITemplateService TemplateService { get; set; }
        public ITestService TestService { get; set; }

        public ActionResult Index()
        {
            List<RespondentListItem> list = new List<RespondentListItem>();
            List<Respondent> respondents = GetSortedRespondents();
            foreach (var respondent in respondents)
            {
                list.Add(new RespondentListItem
                {
                    Respondent = respondent,
                    ExtraInfo = GetExtraInfo(respondent.Id)
                });
            }

            return View(list);
        }

        private List<Respondent> GetSortedRespondents()
        {
            List<Respondent> list = RespondentService.ListRespondents();
            List<Respondent> respondents = new List<Respondent>();
            respondents.AddRange(list.Where(t => !t.Tests.Any()));
            list.RemoveAll(t => !t.Tests.Any());
            respondents.AddRange(list.Where(t => t.Tests.Where(x => x.DateStarted != null && x.DateCompleted == null).Any()));
            list.RemoveAll(t => t.Tests.Where(x => x.DateStarted != null && x.DateCompleted == null).Any());
            respondents.AddRange(list.Where(t => t.Tests.Where(x => x.DateCompleted != null && x.DateEvaluated == null).Any()));
            list.RemoveAll(t => t.Tests.Where(x => x.DateCompleted != null && x.DateEvaluated == null).Any());
            list = list.OrderByDescending(t => t.Tests.Select(x => x.ScorePercent).Sum() / t.Tests.Count()).ToList();
            respondents.AddRange(list);
            return respondents;
        }

        public ActionResult New(Respondent respondent)
        {
            ModelState.Clear();
            return View(respondent);
        }

        public ActionResult Save(Respondent respondent)
        {
            if (!ModelState.IsValid) return View("New", respondent);
            var added = false;
            if (respondent.Id == Guid.Empty)
            {
                var exists = RespondentService.ListRespondents().Where(r => r.Email == respondent.Email).Any();
                if (exists) ModelState.AddModelError(String.Empty, "The email is already in use");
                else
                {
                    RespondentService.AddRespondent(respondent);
                    added = true;
                }
            }
            else
            {
                var existingResp = RespondentService.GetRespondentById(respondent.Id);
                UpdateModel(existingResp);
                RespondentService.SaveRespondent(existingResp);
                added = true;
            }
            if (added)
                return RedirectToAction("Edit", new { id = respondent.Id });
            else return View("New", respondent);
        }

        public ActionResult DeleteRespondent(string id)
        {
            TestService.DeleteTestsByRespondentId(Guid.Parse(id));
            RespondentService.DeleteRespondent(Guid.Parse(id));
            return RedirectToAction("Index");
        }

        public ActionResult DeleteTest(string id)
        {
            string respondentId = TestService.GetTestById(Guid.Parse(id)).RespondentId.ToString();
            TestService.DeleteTest(Guid.Parse(id));
            return RedirectToAction("Edit", new { id = respondentId });
        }

        public ActionResult Edit(string id)
        {
            Respondent respondent = RespondentService.GetRespondentById(Guid.Parse(id));
            RespondentTestsList list = new RespondentTestsList();
            list.Respondent = respondent;
            ExtraInfo info = GetExtraInfo(respondent.Id);
            list.UncompletedTests = info.UnstartedTests;
            list.TestsInProgress = info.TestsInProgress;
            list.UnevaluatedTests = info.UnevaluatedTests;
            var count = TestService.GetByRespondentId(respondent.Id)
                                                .Where(t => t.DateCompleted != null).Count();
            if (count > 0)
                list.Score = TestService.GetByRespondentId(respondent.Id)
                                                    .Where(t => t.DateCompleted != null)
                                                    .Select(t => t.ScorePercent)
                                                    .Sum() / count;
            else
                list.Score = 0;
            list.Tests = new List<Test>();
            list.Tests = TestService.GetByRespondentId(respondent.Id).OrderByDescending(t => t.TestTemplate.Priority).ToList();
            return View(list);
        }

        public ExtraInfo GetExtraInfo(Guid id)
        {
            ExtraInfo info = new ExtraInfo();
            info.UnstartedTests = TestService.GetByRespondentId(id)
                                                .Where(t => t.DateStarted == null && t.DateCompleted == null)
                                                .Count();
            info.TestsInProgress = TestService.GetByRespondentId(id)
                                                .Where(t => t.DateStarted != null && t.DateCompleted == null)
                                                .Count();
            info.UnevaluatedTests = TestService.GetByRespondentId(id)
                                                .Where(t => t.DateCompleted != null && t.DateEvaluated == null)
                                                .Count();

            var count = TestService.GetByRespondentId(id)
                                                .Where(t => t.DateCompleted != null).Count();
            if (count > 0)
                info.Score = TestService.GetByRespondentId(id)
                                                    .Where(t => t.DateCompleted != null)
                                                    .Select(t => t.ScorePercent)
                                                    .Sum() / count;
            else
                info.Score = 0;

            return info;
        }

        public ActionResult AddTest(Guid id)
        {
            RespondentTestTemplateListVM model = new RespondentTestTemplateListVM
            {
                Templates = TemplateService.ListTemplates(),
                RespondentId = id
            };
            return View(model);
        }

        public ActionResult AddTestToRespondent(Guid templateId, Guid respondentId)
        {
            TestTemplate template = TemplateService.GetTemplateById(templateId);

            if (template == null)
                return null;

            Test test = new Test()
            {
                Answers = new List<Answer>(),
                RespondentId = respondentId,
                TestId = template.Id
            };
            
            TestService.AddTest(test);

            return RedirectToAction("Edit", new { id = respondentId });
        }
    }
}
