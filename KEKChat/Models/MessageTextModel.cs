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

        public List<MessageModel> MessageCollection { get; set; } = new List<MessageModel>(0);

        public MessageTextModel() { }

        public MessageTextModel(List<MessageModel> collection)
        {
            MessageCollection = collection;
        }
    }
}