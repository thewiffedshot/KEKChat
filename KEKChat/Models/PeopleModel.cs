using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class PeopleModel
    {
        public static readonly uint userTimeoutInterval = 15; // In seconds.

        public string Username { get; private set; }
        public bool Online { get; private set; }

        public PeopleModel(string username, DateTime lastOnline)
        {
            DateTime current = DateTime.Now;

            Username = username;
            Online = (current - lastOnline).TotalSeconds <= userTimeoutInterval;
        }
    }
}