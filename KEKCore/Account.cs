using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using KEKCore.Contexts;
using KEKCore.Entities;
using KEKCore.Utils;

namespace KEKCore
{
    public class Account
    {
        public UsersDB DBContext { get; private set; }

        public Account()
        {
            DBContext = new UsersDB();
        }

        public Account(UsersDB context)
        {
            DBContext = context;
        }

        public bool UserExists(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            User user = db.Users.SingleOrDefault(u => u.Username == username);

            if (user != null)
                return true;
            return false;
        }

        public void SendHeartbeat(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            db.Users.Single(u => u.Username == username).LastOnline = DateTime.Now;

            db.SaveChanges();
        }

        public bool Authenticate(string username, string password, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

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

        public bool Register(string username, string password, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            User user = db.Users
                .SingleOrDefault(n => n.Username == username);

            if (user == null)
            {
                string[] hashes = PasswordHash.CreateHash(password);

                db.Users.Add(
                    new User
                        {
                            Username = username,
                            PasswordHash = hashes[0],
                            HashSalt = hashes[1],
                            HashIterations = hashes[2]
                        });

                List<MemeEntry> memes = db.MemeStash.Where(u => u.VendorAmount > 0).ToList();

                db.SaveChanges();

                int userID = db.Users.Single(u => u.Username == username).ID;

                foreach (MemeEntry meme in memes)
                {
                    db.OrderWeights.Add(
                        new OrderWeight() { UserID = userID, MemeID = meme.ID, Weight = 0 });
                }

                db.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Transaction> GetTransactions(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            return db.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Meme)
                .Where(
                    t => t.Buyer.Username == username ||
                         t.Seller.Username == username).ToList();
        }
    }
}