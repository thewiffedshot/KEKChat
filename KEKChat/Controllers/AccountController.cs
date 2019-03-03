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
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {     
            if (ModelState.IsValid)
            {
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                 .SqlQuery("SELECT * FROM users WHERE \"Username\"='" + model.Username + "'")
                                 .SingleOrDefault();

                    if (user != null && user.Password == model.Password)
                    {
                        return View("Dashboard");
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert(\"Wrong username and/or password.\");</script>";
                    }
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                 .SqlQuery("SELECT * FROM users WHERE \"Username\"='" + model.Username + "'")
                                 .SingleOrDefault();

                    if (user == null)
                    {
                        db.Users.Add(model);
                        db.SaveChanges();
                    }
                    else
                    {
                        TempData["msg"] = "<script>alert(\"User already registered.\");</script>";
                    }
                }    
            }

            return View(model);
        }
    }
}