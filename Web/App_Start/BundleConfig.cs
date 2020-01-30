using System.Web;
using System.Web.Optimization;

namespace Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      //"~/Scripts/bootstrap.js",
                      //"~/Scripts/respond.js",
                      "~/Content/packages/jobject/pdfobject.js",
                      "~/Content/clipOne/plugins/jQuery-lib/2.0.3/jquery.min.js",
                      "~/Content/clipOne/plugins/jquery-ui/jquery-ui-1.10.2.custom.min.js",
                      "~/Content/clipOne/plugins/bootstrap/js/bootstrap.min.js",
                      "~/Content/clipOne/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                      "~/Content/clipOne/blockUI/jquery.blockUI.js",
                      "~/Content/clipOne/iCheck/jquery.icheck.min.js",
                      "~/Content/clipOne/perfect-scrollbar/src/jquery.mousewheel.js",
                      "~/Content/clipOne/perfect-scrollbar/src/perfect-scrollbar.js",
                      "~/Content/clipOne/less/less-1.5.0.min.js",
                      "~/Content/clipOne/jquery-cookie/jquery.cookie.js",
                      "~/Content/clipOne/bootstrap-colorpalette/js/bootstrap-colorpalette.js",
                      "~/Content/clipOne/js/main.js",
                      "~/Scripts/codemirror-3.01/codemirror.js",
                      "~/Scripts/codemirror-3.0/mode/stex.js",
                      "~/Content/toastr/toastr.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      //"~/Content/bootstrap.css",
                      //"~/Content/Site.css",
                      "~/Content/clipOne/plugins/bootstrap/css/bootstrap.min.css",
                      "~/Content/clipOne/plugins/font-awesome/css/font-awesome.min.css",
                      "~/Content/clipOne/fonts/style.css",
                      "~/Content/clipOne/css/main.css",
                      "~/Content/clipOne/css/main-responsive.css",
                      "~/Content/clipOne/plugins/iCheck/skins/all.css",
                      "~/Content/clipOne/plugins/bootstrap-colorpalette/css/bootstrap-colorpalette.css",
                      "~/Content/clipOne/plugins/perfect-scrollbar/src/perfect-scrollbar.css",
                      "~/Content/clipOne/css/theme_light.css",
                      "~/Content/clipOne/css/print.css",
                      "~/Content/clipOne/plugins/fullcalendar/fullcalendar/fullcalendar.css",
                      "~/Content/codemirror-3.01/codemirror.css",
                      "~/Content/codemirror-3.0/theme/night.css",
                      "~/Content/toastr/toastr.css"
                      ));
        }
    }
}
