using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Models;

namespace KEKChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        //[HttpPost]
        public ActionResult Login()
        {         
            return View();
        }

        //[HttpPost]
        public ActionResult Register()
        {
            return View();
        }
    }
}