using System.Collections.Generic;

namespace SE
{
    public class StockMarket : List<IStock>
    {
        public IStock Get(IStock stock)
        {
            return Find(x => x.MarketID == stock.MarketID);
        }
    }
}
