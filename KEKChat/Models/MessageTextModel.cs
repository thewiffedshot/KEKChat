using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KEKChat.Models
{
    public class MessageTextModel
    {
        [Required, DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public List<Message> MessageCollection { get; set; }

        public MessageTextModel() { }

        public MessageTextModel(List<Message> collection)
        {
            MessageCollection = collection;
        }
    }
}