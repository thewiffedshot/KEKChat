using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KEKChat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}