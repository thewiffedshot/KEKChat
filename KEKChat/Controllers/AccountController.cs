﻿using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using KEKChat.Models;
using KEKChat.CoreAPI.Utils;
using KEKChat.Contexts;

namespace KEKChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && CoreAPI.Account.UserExists(User.Identity.Name))
            {
                return RedirectToAction("Chat", "Home");
            }

            return View();
        }

        public ActionResult SignOut()
        {
            Session["currency"] = null;

            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (CoreAPI.Account.Authenticate(model.Username, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                Session["currency"] = CoreAPI.Session.GetUserCurrency(model.Username);

                return RedirectToAction("Chat", "Home");
            }
            else
            {
                ModelState.AddModelError("LoginError", "Invalid username and/or password.");

                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                if (CoreAPI.Account.Register(model.Username, model.Password))
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("RegError", "User already taken.");
                }
            }

            return View();
        }
    }
}