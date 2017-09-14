using UnityEngine;
using UnityEngine.UI;

namespace SE.UI
{
    public class StockAgent : MonoBehaviour
    {
        [SerializeField]
        private IStock _stockModel;
        private IStockExchange _stockExchange;

        [SerializeField]
        private RawImage _icon;
        [SerializeField]
        private Text _name;
        [SerializeField]
        private Text _currentValueDisplay;

        public void Initialize(IStock model)
        {
            _stockModel = model;
            //_stockExchange = exchange;

            _icon.texture = model.Icon;
            _name.text = model.MarketID;
        }

        private void Update()
        {
            _currentValueDisplay.text = string.Format("{0:C}", _stockModel.CurrentValue);
        }
    }
}
