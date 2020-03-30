using System.Web.Mvc;

namespace Comision.Ui.Areas.User.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult RenderPageHeaderPartial()
        {
            return PartialView(MVC.User.Shared.Views._PageHeaderPartialView);
        }

    

    }
}