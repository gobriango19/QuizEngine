using System.Web;
using System.Web.Optimization;

namespace QuizEngine
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobajax").Include(
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundle/requirejs").Include(
                "~/Scripts/require.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/quizengine").Include(
                "~/Scripts/quizEngine.js"));

            bundles.Add(new ScriptBundle("~/bundles/backbonemarionette").Include(
                "~/Scripts/underscore.js",
                "~/Scripts/backbone.js",
                "~/Scripts/backbone.marionette.js",
                "~/Scripts/backbone-relational.js"));

            bundles.Add(new ScriptBundle("~/bundles/clientspa").Include(
                "~/Scripts/ClientSPA/app.js",
                "~/Scripts/ClientSPA/entities.js",
                "~/Scripts/ClientSPA/Common/common_views.js",
                "~/Scripts/ClientSPA/QuizApp/quiz_app.js",
                "~/Scripts/ClientSPA/QuizApp/List/list_views.js",
                "~/Scripts/ClientSPA/QuizApp/List/list_controller.js",
                "~/Scripts/ClientSPA/QuizApp/Show/show_views.js",
                "~/Scripts/ClientSPA/QuizApp/Show/show_controller.js"));

            // admin pages bundles
            bundles.Add(new ScriptBundle("~/bundles/quizengine/admin/allquizzes").Include(
                "~/Scripts/Admin/allQuizzes.js"));

            bundles.Add(new ScriptBundle("~/bundles/quizengine/admin/editquiz").Include(
                "~/Scripts/Admin/editQuiz.js"));

            bundles.Add(new ScriptBundle("~/bundles/quizengine/admin/editquestion").Include(
                "~/Scripts/Admin/editQuestion.js"));

            bundles.Add(new ScriptBundle("~/bundles/quizengine/admin/editanswer").Include(
                "~/Scripts/Admin/editAnswer.js"));

            bundles.Add(new ScriptBundle("~/bundles/quizengine/admin/editresult").Include(
                "~/Scripts/Admin/editResult.js"));

            // styles
            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                //"~/Content/bootstrap-theme.css",
                "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/content/css/admin").Include(
                "~/Content/bootstrap.css",
                //"~/Content/bootstrap-theme.css",
                "~/Content/Admin/Site.css"));

            bundles.Add(new StyleBundle("~/content/css/clientspa").Include(
                "~/Content/bootstrap.css",
                //"~/Content/bootstrap-theme.css",
                "~/Content/ClientSPA/Site.css"));
        }
    }
}