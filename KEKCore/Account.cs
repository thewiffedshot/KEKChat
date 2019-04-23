using KEKCore.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Utils;
using KEKCore.Entities;

namespace KEKCore
{
    public static class Account
    {
        public static bool UserExists(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users.SingleOrDefault(u => u.Username == username);

                if (user != null)
                    return true;
            }
            return false;
        }

        public static void SendHeartbeat(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                db.Users.SingleOrDefault(u => u.Username == username).LastOnline = DateTime.Now;

                db.SaveChanges();
            }
        }

        public static bool Authenticate(string username, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users
                             .SingleOrDefault(u => u.Username == username);

                return user != null && PasswordHash.ValidatePassword(password, user.PasswordHash, user.HashSalt, user.HashIterations);
            }
        }

        public static bool Register(string username, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users
                                .SingleOrDefault(n => n.Username == username);

                if (user == null)
                {
                    string[] hashes = PasswordHash.CreateHash(password);

                    db.Users.Add(new User { Username = username,
                                            PasswordHash = hashes[0],
                                            HashSalt = hashes[1],
                                            HashIterations = hashes[2]
                    });
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }
    }
}