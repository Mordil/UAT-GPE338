using SE.Models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SE.Factories
{
    public struct StockTypes
    {
        public const string Tech = "tech";
    }

    public class StockFactory
    {
        private readonly Dictionary<string, Texture2D> _iconMapping;

        public StockFactory(Dictionary<string, Texture2D> iconMapping)
        {
            _iconMapping = iconMapping;
        }

        public IStock CreateStockModel<T>(SerializableStock<T> stockToCreate)
            where T : IStock
        {
            switch (stockToCreate.type)
            {
                case StockTypes.Tech:
                    return new TechStock(stockToCreate.marketId, _iconMapping[stockToCreate.type]);

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
