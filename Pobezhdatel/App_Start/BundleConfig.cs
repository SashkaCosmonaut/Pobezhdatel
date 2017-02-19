using System.Web.Optimization;

namespace Pobezhdatel
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Add scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/JQuery/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/signalr").Include("~/Scripts/JQuerySignalR/jquery.signalR-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap_scripts").Include("~/Scripts/Bootstrap/bootstrap.js"));

            // Add styles
            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap_styles").Include("~/Content/Bootstrap/bootstrap.css"));
        }
    }
}