﻿using KEKChat.Contexts;
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
            MessageTextModel messages;
            using (UsersDB db = new UsersDB())
            {
                messages = new MessageTextModel(db.Messages
                                                  .ToList());
            }

            return View("Chat", messages);
        }

        public ActionResult SendMessage()
        {
            return View("Chat");
        }

        public async Task<ActionResult> Home()
        {
            ViewBag.SyncOrAsync = "Asynchronous";

            string savepath = Server.MapPath("~") + "Memes\\";

            MemeScraper scraper = new MemeScraper(savepath, new string[] { "me_irl", "deepfriedmemes", "memes" }, 10);

            await scraper.GetMemesFromSubsAsync();

            return RedirectToAction("Chat");
        }

        public ActionResult Store(MemeModel meme)
        {
            return View();
        }

        public ActionResult Store()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendMessage(MessageTextModel msg)
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