using KEKCore.Contexts;
using KEKCore.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace KEKCore
{
    public static class Session
    {
        public static IEnumerable<User> GetPeopleList()
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Users
                         .OrderBy(u => u.Username)
                         .ToList();
            }
        }

        public static IEnumerable<MemeAsset> GetInventoryList(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                int ownerID = db.Users
                                .Where(u => u.Username == username)
                                .Select(u => u.ID)
                                .SingleOrDefault();

                return db.MemeOwners
                         .Include(mo => mo.MemeEntry)
                         .Where(mo => mo.UserID == ownerID && mo.Amount > 0)
                         .ToList();
            }
        }

        public static decimal GetUserCurrency(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Users
                         .SingleOrDefault(u => u.Username == username)
                         .Currency;
            }
        }
    }
}