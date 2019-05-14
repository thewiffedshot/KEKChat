using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KEKCore.Entities
{
    [Table("memes")]
    public class MemeEntry
    {
        [Key]
        public int ID { get; set; }

        public string ImagePath { get; set; }

        public decimal Price { get; set; }

        public int VendorAmount { get; set; }
        
        public string Subreddit { get; set; }

        public int GoldCount { get; set; }

        public bool NSFW { get; set; }

        public IEnumerable<Message> Messages { get; set; }
        public int InitCount { get; set; }
    }
}