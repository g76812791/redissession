using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedisSession.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SessionHelper<string> session = new SessionHelper<string>();
            session["xiezaiweb3", "12344"] = "test";
            string val = session["xiezaiweb3", "12344"];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}