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

            // Add styles
            bundles.Add(new StyleBundle("~/Content/site").Include("~/Content/site.css"));
        }
    }
}