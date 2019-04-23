using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using KEKCore.Contexts;
using KEKCore.Entities;
using System.Data.Entity;

namespace KEKCore
{
    public static class Chat
    {
        public static void SendMessage(string msg, string username)
        {
            if (string.IsNullOrEmpty(msg) || msg.Count(c => c == ' ') == msg.Length
                                          || msg.Count(c => c == '\n') == msg.Length) return;
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users.SingleOrDefault(u => u.Username == username);

                db.Messages.Add(new Message { UserID = user.ID,
                                                    Text = msg,
                                                    Username = user.Username
                                                });
                db.SaveChanges();
            }
        }

        public static IEnumerable<Message> GetMessages(int lastMessageID)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Messages
                         .Where(m => m.ID > lastMessageID)
                         .Include(m => m.Meme)
                         .ToList();
            }
        }

        public static void SendMeme(int memeID, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        User user = db.Users.SingleOrDefault(u => u.Username == username);

                        db.Messages.Add(new Message {
                                                MemeID = memeID,
                                                UserID = user.ID,
                                                Username = username
                        });

                        MemeAsset asset = db.MemeOwners
                                      .Include(a => a.MemeEntry)
                                      .SingleOrDefault(a => a.MemeID == memeID && a.UserID == user.ID);

                        asset.Amount--;

                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                    }
                }
            }
        }
    }
}