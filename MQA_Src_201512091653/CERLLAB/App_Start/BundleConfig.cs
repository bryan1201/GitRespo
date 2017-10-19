using System.Web;
using System.Web.Optimization;

namespace CERLLAB
{
    public class BundleConfig
    {
        // 如需 Bundling 的詳細資訊，請造訪 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
#if !DEBUG
BundleTable.EnableOptimizations = true;
#endif

            bundles.Add(new StyleBundle("~/Content/jquery.treeview/css").Include(
                 "~/Content/jquery.treeview/jquery.treeview.css"));
            
            //                        "~/Scripts/jquery-2.1.0.min.js"
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-2.1.1.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js",
                        //"~/Scripts/jquery-ui-1.10.4.js",
                        "~/Scripts/jquery-ui-1.10.4.min.js",
                        "~/Scripts/jquery-ui-1.10.1.custom.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/treeview").Include(
                        //"~/Content/jquery.treeview/lib/jquery.js",
                        "~/Content/jquery.treeview/lib/jquery.cookie.js",
                        "~/Content/jquery.treeview/jquery.treeview.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"
                        ));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好實際執行時，請使用 http://modernizr.com 上的建置工具，只選擇您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                        "~/Content/jquery.cycle.all.js",
                        "~/Content/jquery.marquee.js",
                        "~/Content/jquery.marquee.min.js",
                        "~/Content/Site.js",
                        "~/Content/PagedList.css"
                        ));
 
            bundles.Add(new ScriptBundle("~/Content/themes/start/jsui").Include(
                        "~/Content/themes/start/js/jquery-ui-1.10.4.custom.js",
                        "~/Content/themes/start/js/jquery-ui-1.10.4.custom.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/start/css").Include(
                        "~/Content/themes/start/css/jquery-ui*"
                        ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.base.css",
                        "~/Content/themes/base/jquery.ui.all.css",
                        "~/Content/themes/base/jquery.ui.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.menu.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"
                        ));

        }
    }
}