using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KEKCore.Entities
{
    

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

        public decimal Price { get; set; }
    }
}