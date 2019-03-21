using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class PeopleListModel
    {
        public List<User> UsersCollection { get; private set; } = new List<User>(0);
        public List<bool> UsersOnlineStatus { get; private set; } = new List<bool>(0);

        public PeopleListModel()
        {

        }

        public PeopleListModel(List<User> users, List<bool> usersStatus)
        {
            UsersCollection = users;
            UsersOnlineStatus = usersStatus;

            UsersCollection.OrderBy(u => u.LastOnline);
            UsersOnlineStatus.OrderByDescending(u => u.ToString());
        }
    }
}