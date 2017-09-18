using UnityEngine;

namespace SE
{
    public interface IStock
    {
        decimal CurrentValue { get; }
        string MarketID { get; }

        Texture2D Icon { get; }

        void Purchase(decimal quantity);
    }
}
