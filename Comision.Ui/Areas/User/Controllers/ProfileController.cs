using Comision.Helper.Filters;
using System.Web.Mvc;

namespace Comision.Ui.Areas.User.Controllers
{
    [AuthorizeUser]
    public partial class ProfileController : Controller
    {
        // GET: User/Profile
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult SaveProfile()
        {
            return View();
        }
    }
}