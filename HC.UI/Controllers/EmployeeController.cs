using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: EmployeeDefault
        public ActionResult List()
        {
            return View();
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Update()
        {
            return View();
        }
    }
}