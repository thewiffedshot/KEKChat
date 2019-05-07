using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEKCore.Entities
{
    [Table("transactions")]
    public class Transaction
    {
        [Key]
        public int ID { get; set; }

        public User Buyer { get; set; }

        public User Seller { get; set; }

        [ForeignKey("Buyer")]
        public int BuyerID { get; set; }

        [ForeignKey("Seller")]
        public int? SellerID { get; set; }

        public string SellerName { get; set; }

        public decimal Value { get; set; }

        public int Quantity { get; set; }

        public MemeEntry Meme { get; set; }

        [ForeignKey("Meme")]
        public int MemeID { get; set; }

        public string AssetName { get; set; }

        public DateTime TimeStamp { get; set; } 
    }
}
