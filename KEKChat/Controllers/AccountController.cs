﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using KEKChat.Models;
using System.Data.Sql;
using System.Security.Cryptography;

namespace KEKChat.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            if(Session["username"] != null)
            {
                return RedirectToAction("Chat","Home");
            }

            if ((string)TempData["loginfailDisplay"] != "inline")
                TempData["loginfailDisplay"] = "none";

            return View();
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session["username"] = null;
            Session["currency"] = null;

            return View("Login");
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

                    TempData["loginfailDisplay"] = "none";
                    Session["username"] = user.Username;
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
            if ((string)TempData["confirmationDisplay"] != "inline")
                TempData["confirmationDisplay"] = "none";

            if ((string)TempData["usertakenDisplay"] != "inline")
                TempData["usertakenDisplay"] = "none";

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

                        return View("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User already taken.");
                        return View();
                    }
                }    
            }

            return View();
        }
    }

    public class PasswordHash
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;

        public static string[] CreateHash(string password)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            return new string[] { Convert.ToBase64String(hash),
                                  Convert.ToBase64String(salt),
                                  PBKDF2_ITERATIONS.ToString() };
        }

        public static bool ValidatePassword(string password, string passhash, string _salt, string _iterations)
        {
            int iterations = Int32.Parse(_iterations);
            byte[] salt = Convert.FromBase64String(_salt);
            byte[] hash = Convert.FromBase64String(passhash);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}