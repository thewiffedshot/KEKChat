using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using KEKChat.Models;
using KEKCore.Utils;

using KEKChat.Contexts;
using UsersDB = KEKCore.Contexts.UsersDB;

namespace KEKChat.Controllers
{

    public class AccountController : Controller
    {
        static UsersDB db = new UsersDB();

        private readonly KEKCore.Account account = new KEKCore.Account(db);
        private readonly KEKCore.Session session = new KEKCore.Session(db);

        // GET: Account
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated && account.UserExists(User.Identity.Name))
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
            if (account.Authenticate(model.Username, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Username, false);
                Session["currency"] = session.GetUserCurrency(model.Username);

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
                if (account.Register(model.Username, model.Password))
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