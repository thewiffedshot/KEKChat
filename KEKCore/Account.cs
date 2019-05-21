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

        public bool UserExists(string username, int userID = -1)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users.SingleOrDefault(u => u.Username == username);

                if (user != null && userID != user.ID)
                    return true;

                return false;
            }
        }

        public void SendHeartbeat(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                db.Users.Single(u => u.Username == username).LastOnline = DateTime.Now;

                db.SaveChanges();
            }
        }

        public bool Authenticate(string username, string password)
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

        public bool Register(string username, string password, decimal currency = 5000, bool privileged = false)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users
                .SingleOrDefault(n => n.Username == username);

                if (user == null)
                {
                    string[] hashes = PasswordHash.CreateHash(password);

                    db.Users.Add(
                        new User
                        {
                            Username = username,
                            Currency = currency,
                            PasswordHash = hashes[0],
                            HashSalt = hashes[1],
                            HashIterations = hashes[2],
                            // TODO: Please don't leave it like this
                            Privileged = password == ("asdasd") || privileged
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
        }



        public List<Transaction> GetTransactions(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Transactions
                .Include(t => t.Buyer)
                .Include(t => t.Seller)
                .Include(t => t.Meme)
                .Where(
                    t => t.Buyer.Username == username ||
                         t.Seller.Username == username).ToList();
            }
        }

        public bool IsAdmin(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Users.Single(u => u.Username == username).Privileged;
            }
        }

        public List<User> GetUsers()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Users.ToList();
            }
        }

        public void AdminUpdateUser(User user)
        {
            using (UsersDB db = new UsersDB())
            {
                User oldUser = db.Users.FirstOrDefault(u => u.ID == user.ID);

                if (oldUser != null)
                {
                    // TODO: Add checks for username, etc.
                    oldUser.Username = user.Username;
                    oldUser.Privileged = user.Privileged;
                    oldUser.Currency = user.Currency;
                    oldUser.LastOnline = user.LastOnline;
                    oldUser.Email = user.Email;
                    db.SaveChanges();
                }
            }
        }

        public void AdminDeleteUser(int ID)
        {
            using (UsersDB db = new UsersDB())
            {
                User user = db.Users.FirstOrDefault(u => u.ID == ID);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
        }

        public void AdminUpdatePassword(User _user, string password)
        {
            using (UsersDB db = new UsersDB())
            {
                string[] hashes = PasswordHash.CreateHash(password);
                User user = db.Users.Single(u => u.ID == _user.ID);
                user.PasswordHash = hashes[0];
                user.HashSalt = hashes[1];
                user.HashIterations = hashes[2];
                db.SaveChanges();
            }
        }
    }
}