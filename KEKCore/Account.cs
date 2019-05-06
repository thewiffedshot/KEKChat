using KEKCore.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Utils;
using KEKCore.Entities;
using System.Data.Entity;

namespace KEKCore
{
    public static class Account
    {
        public static bool UserExists(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users.Where(u => u.Username == username).SingleOrDefault();

                if (user != null)
                    return true;
            }
            return false;
        }

        public static void SendHeartbeat(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                db.Users.Where(u => u.Username == username).SingleOrDefault().LastOnline = DateTime.Now;

                db.SaveChanges();
            }
        }

        public static bool Authenticate(string username, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users
                             .Where(u => u.Username == username)
                             .SingleOrDefault();

                if (user != null && PasswordHash.ValidatePassword(password, user.PasswordHash, user.HashSalt, user.HashIterations))
                {
                    return true;
                }

                return false;
            }
        }

        public static bool Register(string username, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                var user = db.Users
                                .Where(n => n.Username == username)
                                .SingleOrDefault();

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

        public static IQueryable<Transaction> GetTransactions(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Transactions
                         .Include(t => t.Buyer)
                         .Include(t => t.Seller)
                         .Where(t => t.Buyer.Username == username ||
                                     t.Seller.Username == username).ToList().AsQueryable();
            }
        }
    }
}