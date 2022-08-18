using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CashierDashboard()
        {
            return View();
        }

        public ActionResult ManagerDashboard()
        {
            return View();
        }
    }
}