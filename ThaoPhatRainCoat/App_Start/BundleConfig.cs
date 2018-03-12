using System.Web;
using System.Web.Optimization;

namespace ThaoPhatRainCoat
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Content/js/materialize.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/Angularjs").Include(
                "~/Scripts/angular.min.js",
                "~/Scripts/Apps/app.js"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/w3.css",
                      "~/Content/customCSS/raincoat.css",
                      "~/Content/materialize.min.css"));
        }
    }
}
