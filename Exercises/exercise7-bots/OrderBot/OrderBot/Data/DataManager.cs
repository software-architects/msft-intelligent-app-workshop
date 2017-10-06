using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OrderBot.Data
{
    public class DataManager
    {
        private IEnumerable<Stockitem> stockitems;

        public DataManager()
        {
            this.stockitems = InitializeStockitems();
        }

        private IEnumerable<Stockitem> InitializeStockitems()
        {
            yield return new Stockitem()
            {
                StockItemID = 1,
                StockItemName = "Chocolate Dark",
                ImageUrl = "https://raw.githubusercontent.com/software-architects/msft-intelligent-app-workshop/master/Exercises/exercise7-bots/images/chocolate.jpg",
                RecommendedRetailPrice = 3.2m
            };

            yield return new Stockitem()
            {
                StockItemID = 2,
                StockItemName = "Chocolate with Orange",
                ImageUrl = "https://raw.githubusercontent.com/software-architects/msft-intelligent-app-workshop/master/Exercises/exercise7-bots/images/chocolate.jpg",
                RecommendedRetailPrice = 3.8m
            };

            yield return new Stockitem()
            {
                StockItemID = 3,
                StockItemName = "Chocolate with Cranberry",
                ImageUrl = "https://raw.githubusercontent.com/software-architects/msft-intelligent-app-workshop/master/Exercises/exercise7-bots/images/chocolate.jpg",
                RecommendedRetailPrice = 4.1m
            };

            yield return new Stockitem()
            {
                StockItemID = 10,
                StockItemName = "Red shirt, S",
                ImageUrl = "https://sec.ch9.ms/ch9/56e2/7d81df89-2160-453d-b300-255ef17756e2/Behindscenes_960.jpg",
                RecommendedRetailPrice = 24.9m
            };

            yield return new Stockitem()
            {
                StockItemID = 11,
                StockItemName = "Red shirt, M",
                ImageUrl = "https://sec.ch9.ms/ch9/56e2/7d81df89-2160-453d-b300-255ef17756e2/Behindscenes_960.jpg",
                RecommendedRetailPrice = 24.9m
            };

            yield return new Stockitem()
            {
                StockItemID = 12,
                StockItemName = "Red shirt, L",
                ImageUrl = "https://sec.ch9.ms/ch9/56e2/7d81df89-2160-453d-b300-255ef17756e2/Behindscenes_960.jpg",
                RecommendedRetailPrice = 27.9m
            };

            yield return new Stockitem()
            {
                StockItemID = 13,
                StockItemName = "Red shirt, XL",
                ImageUrl = "https://sec.ch9.ms/ch9/56e2/7d81df89-2160-453d-b300-255ef17756e2/Behindscenes_960.jpg",
                RecommendedRetailPrice = 29.9m
            };
        }

        public Task<ICollection<Stockitem>> GetStockitemsAsync(string searchString)
        {
            searchString = searchString.ToLower();

            var result = from s in stockitems
                         where s.StockItemName.ToLower().Contains(searchString)
                         select s;

            return Task.FromResult<ICollection<Stockitem>>(result.ToList());
        }
    }

    [Serializable]
    public class Stockitem
    {
        public int StockItemID { get; set; }

        public string StockItemName { get; set; }

        public decimal RecommendedRetailPrice { get; set; }

        public string ImageUrl { get; set; }

        public override string ToString()
        {
            return StockItemName;
        }
    }
}