using System.Web;
using System.Web.Optimization;

namespace TestDelivery.UI
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));
           
            bundles.Add(new ScriptBundle("~/bundles/TestCreation").Include(
                "~/Scripts/jquery-1.*",
                "~/Scripts/jquery-ui*",
                "~/scripts/jQueryDisableSelection.js",
                "~/scripts/Questions.js",
                "~/scripts/jquery.autosize-min.js",
                "~/scripts/Markdown.Converter.js")
               .IncludeDirectory("~/Scripts/Lists", "*.js", false));


            bundles.Add(new ScriptBundle("~/bundles/TestDelivery").Include(
                "~/Scripts/jquery-1.*",
                "~/Scripts/jquery-ui*",
                "~/Scripts/TestTaking.js",
                "~/Scripts/QuestionTimer.js",
                "~/scripts/jquery.autosize-min.js",
                "~/scripts/Markdown.Converter.js"));

            bundles.Add(new StyleBundle("~/Content/css/admin-css")
                .Include("~/Content/css/Admin.css"));

            bundles.Add(new StyleBundle("~/Content/css/respondent-css")
                .Include("~/Content/css/Respondent.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css")
                .Include("~/Content/themes/base/jquery-ui-*"));
        }
    }
}