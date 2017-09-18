using UnityEngine;

namespace SE.Models
{
    public abstract class BaseStock : IStock
    {
        abstract public decimal CurrentValue { get; }

        public string MarketID { get; private set; }

        public Texture2D Icon { get; private set; }

        public BaseStock(string marketID, Texture2D icon)
        {
            MarketID = marketID;
            Icon = icon;
        }

        public abstract void Purchase(decimal quantity);
    }
}
