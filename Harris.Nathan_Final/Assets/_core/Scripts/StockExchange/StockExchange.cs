namespace SE
{
    public class StockExchange : BaseStockExchange
    {
        public StockExchange(StockMarket stockMarket)
            : base(stockMarket)
        {
        }

        public override void PurchaseStock(IStock stock, decimal numOfStock)
        {
            StockMarket.Get(stock).Purchase(numOfStock);
        }

        public override decimal GetCurrentStockValue(IStock stock)
        {
            return StockMarket.Get(stock).CurrentValue;
        }
    }
}
