using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class PeopleListModel
    {
        public static readonly uint userTimeoutInterval = 15; // In seconds.

        public List<PeopleModel> People { get; private set; } = new List<PeopleModel>(0);

        public PeopleListModel()
        {

        }

        public PeopleListModel(List<User> users, DateTime current)
        {
            foreach (User user in users)
                People.Add(new PeopleModel(user.Username, (current - user.LastOnline).TotalSeconds <= userTimeoutInterval));

            var people = People
                         .Where(p => p.Online)
                         .OrderBy(p => p.Username)
                         .Select(p => p)
                         .ToList();

            var offline = People
                          .Where(p => !p.Online)
                          .OrderBy(p => p.Username)
                          .Select(p => p)
                          .ToList();

            var list = people.Concat(offline).ToList();

            People = list;
        }
    }
}