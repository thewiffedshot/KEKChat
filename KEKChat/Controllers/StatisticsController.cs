using DevExpress.Web.Mvc;
using System.Web.Mvc;

using KEKCore.Contexts;
using System;

namespace KEKChat.Controllers
{

    public class StatisticsController : Controller
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
            if (ModelState.IsValid)
            {
                try
                {
                    account.Register(item.Username, "123456");
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
        public ActionResult AdminPanelPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] KEKCore.Entities.User item)
        {
            if (ModelState.IsValid)
            {
                try
                {
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
        public ActionResult AdminPanelPartialDelete(int ID)
        {
            if (ID >= 0)
            {
                try
                {
                    account.AdminDeleteUser(ID);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_AdminPanelPartial", account.GetUsers());
        }
    }
}