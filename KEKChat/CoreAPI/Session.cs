using KEKChat.Contexts;
using KEKChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.CoreAPI
{
    public static class Session
    {
        public static PeopleListModel GetPeopleListModel()
        {
            using (UsersDB db = new UsersDB())
            {
                return new PeopleListModel(db.Users
                                             .OrderBy(u => u.Username)
                                             .ToList(), DateTime.Now);
            }
        }

        public static List<MemeAsset> GetInventoryList(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                int ownerID = db.Users
                                .Where(u => u.Username == username)
                                .Select(u => u.ID)
                                .SingleOrDefault();

                return db.MemeOwners
                         .Where(mo => mo.UserID == ownerID && mo.Amount > 0)
                         .ToList();
            }
        }

        public static decimal GetUserCurrency(string username)
        {
            using (UsersDB db = new UsersDB())
            {
                return db.Users
                         .Where(u => u.Username == username)
                         .SingleOrDefault()
                         .Currency;
            }
        }
    }
}