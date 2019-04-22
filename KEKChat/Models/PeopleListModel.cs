using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Entities;

namespace KEKChat.Models
{
    public class PeopleListModel
    {
        

        public List<PeopleModel> People { get; private set; } = new List<PeopleModel>(0);

        public PeopleListModel()
        {

        }

        public PeopleListModel(IEnumerable<User> users)
        {
            DateTime current = DateTime.Now;

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