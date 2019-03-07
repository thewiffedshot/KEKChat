using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KEKChat.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Chat()
        {
            MessageText messages;
            using (UsersDB db = new UsersDB())
            {
                messages = new MessageText(db.Messages
                                             .ToList());
            }

            return View("Chat", messages);
        }

        public ActionResult SendMessage()
        {
            return View("Chat");
        }

        [HttpPost]
        public ActionResult SendMessage(MessageText msg)
        {
            if (ModelState.IsValid)
            {
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                 .Where(u => u.Username == User.Identity.Name)
                                 .SingleOrDefault();
                    db.Messages.Add(new Message(msg.Text, user));
                    db.SaveChanges();
                }
            }
            
            return RedirectToAction("Chat");
        }
    }
}