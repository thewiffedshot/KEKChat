using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using KEKChat.Models;

namespace KEKChat.Models
{
    [Table("memes")]
    public class MemeModel
    {
        [Key]
        public int ID { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; }
    }
}