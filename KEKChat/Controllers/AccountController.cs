using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
            if ((string)TempData["loginfailDisplay"] != "inline")
                TempData["loginfailDisplay"] = "none";

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {     
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users
                                .SqlQuery("SELECT * FROM users WHERE \"Username\"='" + model.Username + "'")
                                .SingleOrDefault();

                if (user != null && PasswordHash.ValidatePassword(model.Password, user.Password))
                {
                    return View("Dashboard");
                }
                else
                {
                    TempData["loginfailDisplay"] = "inline";
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
                        db.Users.Add(new User(model.Username, PasswordHash.CreateHash(model.Password)));
                        db.SaveChanges();
                        db.Dispose();

                        TempData["confirmationDisplay"] = "none";

                        return View("Login");
                    }
                    else
                    {
                        TempData["usertakenDisplay"] = "inline";
                        return View();
                    }
                }    
            }
            else
            {
                TempData["confirmationDisplay"] = "inline";
            }

            return View();
        }
    }

    public class PasswordHash
    {
        public const int SALT_BYTE_SIZE = 24;
        public const int HASH_BYTE_SIZE = 24;
        public const int PBKDF2_ITERATIONS = 1000;

        public const int ITERATION_INDEX = 0;
        public const int SALT_INDEX = 1;
        public const int PBKDF2_INDEX = 2;

        public static string CreateHash(string password)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return PBKDF2_ITERATIONS + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

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