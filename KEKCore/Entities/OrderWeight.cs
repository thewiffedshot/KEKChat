using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KEKCore.Entities
{
    [Table("orderWeights")]
    public class OrderWeight
    {
        [Key]
        public int ID { get; set; }

        public User User { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public MemeEntry MemeEntry { get; set; }

        [ForeignKey("MemeEntry")]
        public int MemeID { get; set; }

        public decimal Weight { get; set; }
    }
}
