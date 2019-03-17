using KEKChat.Contexts;
using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Utils;
using System.Threading.Tasks;

namespace KEKChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            UpdateUserCurrencyLabel();

            return View("Chat");
        }

        /*public ActionResult SendMessage()
        {
            return View("Chat");
        }*/

        public async Task<ActionResult> Home()
        {
            ViewBag.SyncOrAsync = "Asynchronous";

            string savepath = Server.MapPath("~") + "Memes\\";

            MemeScraper scraper = new MemeScraper(savepath, new string[] { "me_irl", "memes" }, 10);

            await scraper.GetMemesFromSubsAsync();

            return RedirectToAction("Chat");
        }

        public ActionResult Store(MemeModel meme)
        {
            return View();
        }

        public ActionResult StoreInit()
        {
            UpdateUserCurrencyLabel();

            List<MemeEntry> memes = new List<MemeEntry>(0);

            using (UsersDB db = new UsersDB())
            {
                memes = db.MemeStash
                          .Where(meme => meme.VendorAmount > 0)
                          .ToList();

            }

            return View("Store", new MemeModel(memes));
        }

        [HttpPost]
        public ActionResult SendMessage(string msg)
        {
            if (msg != null)
            {
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                    .Where(u => u.Username == User.Identity.Name)
                                    .SingleOrDefault();
                    db.Messages.Add(new Message(msg, user));
                    db.SaveChanges();
                }
            }

            return GetMessages();
        }

        public ActionResult GetMessages()
        {
            MessageTextModel msg = new MessageTextModel();

            using (UsersDB db = new UsersDB())
            {
                msg = new MessageTextModel(db.Messages.ToList());
            }

            return PartialView("_ChatView", msg);
        }

        [HttpPost]
        public ActionResult BuyMeme(MemeModel meme)
        {
            //TODO
            return StoreInit();
        }

        public void UpdateUserCurrencyLabel()
        {
            using (UsersDB db = new UsersDB())
            {
                Session["currency"] = db.Users
                                        .Where(u => u.Username == User.Identity.Name)
                                        .SingleOrDefault()
                                        .Currency;
                
            }
        }
    }
}