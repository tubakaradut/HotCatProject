using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class AuthenticationController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}