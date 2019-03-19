using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KEKChat.Models;
using System.Data.Sql;
using System.Security.Cryptography;
using KEKChat.Utils;
using KEKChat.Contexts;

namespace KEKChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            if(User.Identity.IsAuthenticated && UserExists(User.Identity.Name))
            {
                UpdateUserStatus(true, User.Identity.Name);

                return RedirectToAction("Chat", "Home");
            }

            return View();
        }

        private bool UserExists(string name)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users.Where(u => u.Username == name).SingleOrDefault();

                if (user != null)
                    return true;
            }
            return false;
        }

        public ActionResult SignOut()
        {
            Session["currency"] = null;

            UpdateUserStatus(false, User.Identity.Name);

            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        public ActionResult UpdateUserStatus(bool isOnline, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                db.Users
                  .Where(u => u.Username == username)
                  .Single().IsOnline = isOnline;

                db.SaveChanges();
            }

            return null;
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {

            using (UsersDB db = new UsersDB())
            {
                var user = db.Users
                                .Where(u => u.Username == model.Username)
                                .SingleOrDefault();

                if (user != null && PasswordHash.ValidatePassword(model.Password, user.PasswordHash, user.HashSalt, user.HashIterations))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, false);

                    UpdateUserStatus(true, user.Username);

                    Session["currency"] = user.Currency;
                    return RedirectToAction("Chat", "Home");
                }
                else
                {
                    ModelState.AddModelError("LoginError", "Invalid username and/or password.");
                    return View();
                }
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
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                 .Where(n => n.Username == model.Username) 
                                 .SingleOrDefault();

                    if (user == null)
                    {
                        string[] hashes = PasswordHash.CreateHash(model.Password);

                        db.Users.Add(new User(model.Username, hashes[0], hashes[1], hashes[2]));
                        db.SaveChanges();
                        db.Dispose();

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("RegError", "User already taken.");
                        return View();
                    }
                }    
            }

            return View();
        }
    }
}