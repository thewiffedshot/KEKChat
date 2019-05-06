using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace KEKChat.Controllers
{
    public class StatisticsController : Controller
    {
        public ActionResult Dashboard()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            return PartialView("_GridViewPartial", KEKCore.Account.GetTransactions(User.Identity.Name));
        }
    }
}