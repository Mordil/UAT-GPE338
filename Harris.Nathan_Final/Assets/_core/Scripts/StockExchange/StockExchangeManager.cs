using SE.Factories;
using SE.Models;
using SE.UI;
using System.Collections.Generic;
using UnityEngine;

namespace SE
{
    public class StockExchangeManager : MonoBehaviour
    {
        [SerializeField]
        private int _numOfStockOptions = 10;

        [SerializeField]
        private Texture2D _placeholderIcon;
        [SerializeField]
        private GameObject _stockPrefab;
        [SerializeField]
        private Transform _stockListDisplay;

        private void Start()
        {
            var factory = new StockFactory(new Dictionary<string, Texture2D>() { { StockTypes.Tech, _placeholderIcon } });

            for (int i = 0; i < _numOfStockOptions; i++)
            {
                var stock = Instantiate(_stockPrefab) as GameObject;
                var agent = stock.GetComponent<StockAgent>();
                var model = new SerializableStock<TechStock>()
                {
                    startingValue = 7.5m,
                    marketId = string.Format("PLA:{0}", i + 1),
                    type = StockTypes.Tech
                };
                stock.name = model.marketId;
                agent.Initialize(factory.CreateStockModel(model));
                stock.transform.SetParent(_stockListDisplay, false);
            }
        }
    }
}
