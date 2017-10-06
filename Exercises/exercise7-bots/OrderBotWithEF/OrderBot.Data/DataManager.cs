using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderBot.Data
{
    public class DataManager
    {

        public async Task<ICollection<Stockitem>> GetStockitemsAsync(string searchString)
        {
            using (var db = new WWIContext())
            {
                var result = await (from s in db.Stockitems
                                    where s.StockItemName.Contains(searchString)
                                    select s).ToListAsync();

                return result;
            }
        }

    }
}
