using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestDelivery.DAL;
using TestDelivery.DAL.Questions;

namespace TestDelivery.UI.Helpers
{
    public class QuestionViewService : IQuestionViewService
    {
        public string GetTestPartialViewPath(Question question)
        {
            return GetPartialViewPath(question, "Test");
        }

        public string GetEditPartialViewPath(Question question)
        {
            return GetPartialViewPath(question, "Edit");
        }

        public string GetEvaluatePartialViewPath(Question question)
        {
            return GetPartialViewPath(question, "Evaluate");
        }

        private string GetPartialViewPath(Question question, string viewName)
        {
            var path = string.Format("~/Views/{0}/{1}.cshtml", question.GetType().Name, viewName);
            if (File.Exists(HttpContext.Current.Server.MapPath(path)))
                return path;

            return string.Format("~/Views/Question/{0}.cshtml", viewName);
        }

        public IEnumerable<string> GetQuestionControllerNames()
        {
            //return this
            return Question.QuestionTypes
                           .Where(qt => !qt.IsAbstract)
                           .Select(qt => qt.Name);
        }

    }

    public interface IQuestionViewService
    {
        string GetTestPartialViewPath(Question q);
        string GetEditPartialViewPath(Question q);
        string GetEvaluatePartialViewPath(Question q);
        IEnumerable<string> GetQuestionControllerNames();
    }

}
