﻿using DevExpress.Web.Mvc;
using System.Web.Mvc;
using System.Linq;
using KEKCore.Contexts;
using System;
using KEKCore.Entities;
using KEKChat.Models;

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
            return View();
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
        public ActionResult AdminPanelPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] KEKCore.Entities.User item)
        {
            var password = EditorExtension.GetValue<string>("Password").TrimStart();

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(password))
                        account.Register(item.Username, "123456", item.Currency, item.Privileged);
                    else
                        account.Register(item.Username, password, item.Currency, item.Privileged);
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
        
        public ActionResult EditButtonClick(int id)
        {
            User user = account.GetUsers().Where(u => u.ID == id).SingleOrDefault();

            return View("EditUserView", new EditUserModel
            {
                UserID = id,
                Username = user.Username,
                EmailName = user.Email == null ? "" : user.Email.Split('@')[0],
                EmailDomain = user.Email == null ? "" : user.Email.Split('@')[1]
            });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AdminPanelPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] KEKCore.Entities.User item)
        {
            var password = EditorExtension.GetValue<string>("Password").TrimStart();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(password))
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
                if (!account.UserExists(userInfo.Username))
                {
                    User user = new User
                    {
                        ID = userInfo.UserID,
                        Username = userInfo.Username,
                        Email = userInfo.EmailName + "@" + userInfo.EmailDomain,
                        PasswordHash = "dummy",
                        HashSalt = "dummy",
                        HashIterations = "dummy"
                    };

                    account.AdminUpdateUser(user);
                    account.AdminUpdatePassword(user, userInfo.Password);

                    return View("Dashboard");
                }
                else
                {
                    ModelState.AddModelError("UsernameTakenError", language_strings.UsernameTakenError);
                }
            }

            return View("EditUserView");
        }
    }
}