using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KEKChat.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public ActionResult Chat()
        {
            MessageText messages;
            using (UsersDB db = new UsersDB())
            {
                messages = new MessageText(db.Messages
                                             .SqlQuery("SELECT * FROM messages LIMIT 50")
                                             .ToList());
            }

            return RedirectToAction("Chat", messages);
        }

        public ActionResult Chat(MessageText msgs)
        {

            return View("Chat");
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
                                 .SqlQuery("SELECT * FROM users WHERE \"Username\"='" + Session["username"] + "'")
                                 .SingleOrDefault();
                    db.Messages.Add(new Message(msg.Text, user));
                    db.SaveChanges();
                }
            }
            

            return RedirectToAction("Chat");
        }
    }
}