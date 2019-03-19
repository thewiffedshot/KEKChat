using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class PeopleListModel
    {
        public List<User> usersCollection = new List<User>(0);

        public PeopleListModel()
        {

        }

        public PeopleListModel(List<User> users)
        {
            usersCollection = users;
        }
    }
}