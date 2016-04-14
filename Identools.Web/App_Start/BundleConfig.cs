using System.Web.Optimization;

namespace Identools.Web
{
    public class BundlingConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/react-components.jsx").IncludeDirectory("~/Scripts/components/", "*.jsx"));
        }
    }
}