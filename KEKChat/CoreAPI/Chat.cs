using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using KEKChat.Contexts;
using KEKChat.Models;

namespace KEKChat.CoreAPI
{
    public static class Chat
    {
        public static void SendMessage(string msg, string username)
        {
            if (msg != null && msg != "" && msg.Count(c => c == ' ') != msg.Length && msg.Count(c => c == '\n') != msg.Length)
            {
                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                    .Where(u => u.Username == username)
                                    .SingleOrDefault();
                    db.Messages.Add(new Message(msg, user));
                    db.SaveChanges();
                }
            }
        }

        public static MessageTextModel GetMessages(int lastMessageID)
        {
            using (UsersDB db = new UsersDB())
            {
                var query = from messages in db.Messages
                            where messages.ID > lastMessageID
                            from memes in db.MemeStash.Where(m => m.ID == messages.MemeID).DefaultIfEmpty()
                            orderby messages.ID
                            select new MessageModel { Username = messages.Username, Text = messages.Text, Date = messages.Date, ImageSource = memes.ImagePath, ID = messages.ID };

                return new MessageTextModel(query.ToList());
            }
        }

        public static void SendMeme(string memeID, string username)
        {
            int MemeID = int.Parse(memeID);

            using (TransactionScope scope = new TransactionScope())
            {

                using (UsersDB db = new UsersDB())
                {
                    var user = db.Users
                                 .Where(u => u.Username == username)
                                 .SingleOrDefault();

                    db.Messages.Add(new Message(MemeID, user));

                    var asset = db.MemeOwners
                                  .Where(a => a.MemeID == MemeID && a.UserID == user.ID)
                                  .SingleOrDefault();

                    asset.Amount--;

                    db.SaveChanges();
                }

                scope.Complete();
            }
        }
    }
}