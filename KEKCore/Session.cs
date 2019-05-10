using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using KEKCore.Contexts;
using KEKCore.Entities;

namespace KEKCore
{
    public class Session
    {
        public UsersDB DBContext { get; private set; }

        public Session()
        {
            DBContext = new UsersDB();
        }

        public Session(UsersDB context)
        {
            DBContext = context;
        }

        public IEnumerable<User> GetPeopleList(UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            return db.Users
                     .OrderBy(u => u.Username)
                     .ToList();
        }

        public IEnumerable<MemeAsset> GetInventoryList(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            int ownerId = db.Users
                .Where(u => u.Username == username)
                .Select(u => u.ID)
                .SingleOrDefault();

            return db.MemeOwners
                .Include(mo => mo.MemeEntry)
                .Where(mo => mo.UserID == ownerId && mo.Amount > 0)
                .ToList();
        }

        public decimal GetUserCurrency(string username, UsersDB db = null)
        {
            if (db == null)
                db = DBContext;

            return db.Users
                .Single(u => u.Username == username)
                .Currency;
        }
    }
}