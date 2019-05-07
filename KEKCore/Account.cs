using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using KEKCore.Contexts;
using KEKCore.Entities;
using KEKCore.Utils;

namespace KEKCore
{
    public static class Account
    {
        public static bool UserExists(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users.SingleOrDefault(u => u.Username == username);

                if (user != null)
                    return true;
            }
            return false;
        }

        public static void SendHeartbeat(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                db.Users.Single(u => u.Username == username).LastOnline = DateTime.Now;

                db.SaveChanges();
            }
        }

        public static bool Authenticate(string username, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users
                             .SingleOrDefault(u => u.Username == username);

                if (user != null && PasswordHash.ValidatePassword(
                        password,
                        user.PasswordHash,
                        user.HashSalt,
                        user.HashIterations))
                {
                    user.Currency += 1000;

                    db.SaveChanges();

                    return true;
                }

                return false;
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

        public static List<Transaction> GetTransactions(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Transactions
                    .Include(t => t.Buyer)
                    .Include(t => t.Seller)
                    .Include(t => t.Meme)
                    .Where(t => t.Buyer.Username == username ||
                                t.Seller.Username == username).ToList();
            }
        }
    }
}