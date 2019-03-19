using KEKChat.Contexts;
using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KEKChat.Utils;
using System.Threading.Tasks;
using System.Transactions;

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

        public ActionResult GetPeople()
        {
            List<User> people;

            using (UsersDB db = new UsersDB())
            {
                db.Users
                  .Where(u => u.Username == User.Identity.Name)
                  .SingleOrDefault().IsOnline = true;

                db.SaveChanges();

                people = db.Users
                           .OrderByDescending(u => u.IsOnline)
                           .ToList(); 
            }

            return PartialView("_PeopleList", new PeopleListModel(people));
        }

        [HttpPost]
        public ActionResult BuyMeme(MemeModel meme, string buy)
        {
            int memeID = int.Parse(buy);

            using(TransactionScope scope = new TransactionScope())
            {
                using (UsersDB db = new UsersDB())
                {
                    User user = db.Users
                                  .Where(u => u.Username == User.Identity.Name)
                                  .SingleOrDefault();

                    decimal userCurrency = user.Currency;

                    MemeEntry currentMeme = db.MemeStash
                                       .Where(u => u.ID == memeID)
                                       .SingleOrDefault();

                    decimal memePrice = currentMeme.Price;

                    decimal totalPrice = memePrice * meme.Quantity;

                    if (userCurrency >= totalPrice && currentMeme.VendorAmount >= meme.Quantity && meme.Quantity > 0)
                    {

                        user.Currency -= totalPrice;
                        currentMeme.VendorAmount -= meme.Quantity;

                        MemeOwner owner = new MemeOwner(user, currentMeme, meme.Quantity);

                        db.MemeOwners.Add(owner);
                        db.SaveChanges();
                    }
                }

                scope.Complete();
            }
            

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