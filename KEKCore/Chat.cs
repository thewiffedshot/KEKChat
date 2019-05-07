using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKCore
{
    public static class Chat
    {
        public static void SendMessage(string msg, string username)
        {
            if (!string.IsNullOrEmpty(msg) && msg.Count(c => c == ' ') != msg.Length
                                           && msg.Count(c => c == '\n') != msg.Length)
            {
                using (UsersDB db = new UsersDB())
                {
                    User user = db.Users
                                    .Single(u => u.Username == username);
                    db.Messages.Add(new Message { UserID = user.ID,
                                                  Text = msg
                    });
                    db.SaveChanges();
                }
            }
        }

        public static IEnumerable<Message> GetMessages(int lastMessageId)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Messages
                         .Where(m => m.ID > lastMessageId)
                         .Include(m => m.User)
                         .Include(m => m.Meme)
                         .ToList();
            }
        }

        public static void SendMeme(int memeId, string username)
        {
            using (UsersDB db = new UsersDB())
            {
                using (DbContextTransaction trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        User user = db.Users
                                    .SingleOrDefault(u => u.Username == username);

                        db.Messages.Add(new Message { MemeID = memeId,
                                                      UserID = user.ID
                        });

                        MemeAsset asset = db.MemeOwners
                                      .Include(a => a.MemeEntry)
                                      .Single(a => a.MemeID == memeId && a.UserID == user.ID);

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