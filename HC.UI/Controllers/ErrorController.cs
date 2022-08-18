using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HC.UI.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult AuthenticationError()
        {
            return View();
        }
    }
}