using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Models;
using System.Data.Sql;

namespace KEKChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {         
            return View();
        }

        [HttpPost]
        public ActionResult Register(LoginModel model)
        {
            UsersDB db = new UsersDB();

            db.Entry(model);

            return View();
        }
    }
}