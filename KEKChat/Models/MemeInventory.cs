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
    public class MemeInventory
    {
        [Key]
        public int ID { get; set; }
        public string ImagePath { get; set; }
        public int Price { get; set; }
        public int VendorAmount { get; set; }

        public MemeInventory(string path, int price, int vendorAmount)
        {
            ImagePath = path;
            Price = price;
            VendorAmount = vendorAmount;
        }

        public MemeInventory()
        {

        }
    }
}