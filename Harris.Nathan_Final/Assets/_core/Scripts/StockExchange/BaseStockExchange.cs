namespace SE
{
    public abstract class BaseStockExchange : IStockExchange
    {
        protected StockMarket StockMarket;

        public BaseStockExchange(StockMarket stockMarket)
        {
            StockMarket = stockMarket;
        }

        public abstract void PurchaseStock(IStock stock, decimal quantity);
        public abstract decimal GetCurrentStockValue(IStock stock);
    }
}
