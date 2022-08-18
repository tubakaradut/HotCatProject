using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult CompleteOrder()
        {
            return View();
        }
        public ActionResult CompleteOrderReport()
        {
            return View();
        }
    }
}