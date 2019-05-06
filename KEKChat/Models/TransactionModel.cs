using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KEKCore.Entities;

namespace KEKChat.Models
{
    public class TransactionModel
    {
        public int ID { get; set; }

        public string BuyerName { get; set; }
        public string SellerName { get; set; }

        public decimal Value { get; set; }
        public int Quantity { get; set; }

        public string AssetName { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}