using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class PeopleModel
    {
        public string Username { get; private set; }
        public bool Online { get; private set; }

        public PeopleModel(string username, bool online)
        {
            Username = username;
            Online = online;
        }
    }
}