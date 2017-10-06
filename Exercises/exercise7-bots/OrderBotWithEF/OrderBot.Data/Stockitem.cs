using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderBot.Data
{
    [Serializable]
    [Table("Stockitems", Schema = "Warehouse")]
    public class Stockitem
    {
        public int StockItemID { get; set; }

        public string StockItemName { get; set; }

        public string Size { get; set; }

        public decimal RecommendedRetailPrice { get; set; }

        public override string ToString()
        {
            return StockItemName;
        }
    }
}
