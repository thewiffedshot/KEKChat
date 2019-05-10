using System.Web.Mvc;

using KEKCore.Contexts;

namespace KEKChat.Controllers
{

    public class StatisticsController : Controller
    {
        private static UsersDB db = new UsersDB();

        private readonly KEKCore.Account account = new KEKCore.Account(db);

        public ActionResult Dashboard()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            return PartialView("_GridViewPartial", account.GetTransactions(User.Identity.Name));
        }
    }
}