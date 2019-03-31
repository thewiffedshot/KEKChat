using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class MessageModel
    {
        public string Text { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Username { get; set; }

        public string ImageSource { get; set; } = "";
    }
}