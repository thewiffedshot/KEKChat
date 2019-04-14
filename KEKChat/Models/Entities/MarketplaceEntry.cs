namespace KEKChat.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Linq;

    [Table("marketplace")]
    public class MarketplaceEntry
    {
        [Key]
        public int ID { get; set; }

        public MemeAsset MemeAsset { get; set; }
        [ForeignKey("MemeAsset")]
        public int AssetID { get; set; }

        public User User { get; set; }
        [ForeignKey("User")]
        public int SellerID { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }


        public MarketplaceEntry(MemeAsset asset, User Seller, int quantity, int price)
        {
            MemeAsset = asset;
            AssetID = MemeAsset.ID;
            User = Seller;
            SellerID = User.ID;
            Quantity = quantity;
            Price = price;
        }

        public MarketplaceEntry() { }
    }
}