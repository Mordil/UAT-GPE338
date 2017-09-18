namespace SE
{
    public interface IStockExchange
    {
        void PurchaseStock(IStock stock, decimal quantity);
        decimal GetCurrentStockValue(IStock stock);
    }
}
