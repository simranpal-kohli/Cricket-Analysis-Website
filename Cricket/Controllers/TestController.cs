using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cricket.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult TeamDetail()
        {
            return View();
        }
    }
}