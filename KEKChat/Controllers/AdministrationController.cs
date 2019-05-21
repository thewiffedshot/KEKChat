using System;
using System.Linq;
using System.Web.Mvc;

using DevExpress.Web.Mvc;

using KEKChat.Models;

using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKChat.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        private static UsersDB db = new UsersDB();

        private readonly KEKCore.Account account = new KEKCore.Account(db);
        private readonly KEKCore.Session session = new KEKCore.Session(db);

        public ActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            return PartialView("_GridViewPartial", account.GetTransactions(User.Identity.Name));
        }

        public void UpdateUserCurrencyLabel()
        {
            Session["currency"] = session.GetUserCurrency(User.Identity.Name);
        }

        [ValidateInput(false)]
        public ActionResult AdminPanelPartial()
        {
            UpdateUserCurrencyLabel();

            if (account.IsAdmin(User.Identity.Name))
                return PartialView("_AdminPanelPartial", account.GetUsers());
            return null;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AdminPanelPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] User item)
        {
            var password = EditorExtension.GetValue<string>("Password").TrimStart();

            if (ModelState.IsValid)
            {
                try
                {
                    account.Register(
                        item.Username,
                        string.IsNullOrWhiteSpace(password) ? "123456" : password,
                        item.Currency,
                        item.Privileged);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_AdminPanelPartial", account.GetUsers());
        }
        
        public ActionResult EditUser(int userID)
        {
            User user = account.GetUser(userID);

            string email = "@";

            if (user.Email != null)
                email = user.Email;

            string[] splitEmail = email.Split('@');

            return View("EditUserView", new EditUserModel
            {
                UserID = userID,
                Username = user.Username,
                Email = new EmailInfo {
                    EmailName = splitEmail[0],
                    EmailDomain = splitEmail[1]
                },
                Privileged = user.Privileged,
                Currency = user.Currency
            });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AdminPanelPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] User item)
        {
            string password = EditorExtension.GetValue<string>("Password").TrimStart();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(password))
                        account.AdminUpdatePassword(item, password);

                    account.AdminUpdateUser(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("_AdminPanelPartial", account.GetUsers());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AdminPanelPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] KEKCore.Entities.User item)
        {
            if (item.ID >= 0)
            {
                try
                {
                    account.AdminDeleteUser(item.ID);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_AdminPanelPartial", account.GetUsers());
        }

        [HttpPost]
        public ActionResult UpdateUserInfo(EditUserModel userInfo)
        {
            if (ModelState.IsValid)
            {
                if (!account.UserExists(userInfo.Username, userInfo.UserID))
                {
                    User user = new User
                    {
                        ID = userInfo.UserID,
                        Username = userInfo.Username,
                        Email = userInfo.Email.EmailName + "@" + userInfo.Email.EmailDomain,
                        Privileged = userInfo.Privileged,
                        PasswordHash = "dummy",
                        HashSalt = "dummy",
                        HashIterations = "dummy",
                        Currency = userInfo.Currency
                    };

                    account.AdminUpdateUser(user);
                    account.AdminUpdatePassword(user, userInfo.Password);

                    return Dashboard();
                }
                else
                {
                    ModelState.AddModelError("UsernameTakenError", language_strings.UsernameTakenError);
                }
            }

            return EditUser(userInfo.UserID);
        }
    }
}