using System.Web.Optimization;

namespace Comision.Ui
{
    //dfdfdf
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/assets/global/plugins/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusive").Include(
                                    "~/Scripts/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryAjaxForm").Include(
                        "~/Content/assets/global/plugins/AjaxForm/jquery.form.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/myvalidate").Include(
                        "~/Scripts/my-validate.js"));


            bundles.Add(new ScriptBundle("~/bundles/PersianDate").Include(
                  "~/Scripts/PersianDatePicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/persianDate/css").Include(
                 "~/Content/assets/global/plugins/persiancalendar/PersianDatePicker.min.css"));
        }
    }
}
