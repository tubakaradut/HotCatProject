using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class PageController : Controller
    {
        // GET: Page
        public ActionResult List()
        {
            return View();
        }
        public ActionResult PageRole()
        {
            return View();
        }
    }
}