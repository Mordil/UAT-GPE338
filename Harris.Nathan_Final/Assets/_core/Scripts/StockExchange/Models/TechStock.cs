using System;
using UnityEngine;

namespace SE.Models
{
    public class TechStock : BaseStock
    {
        private decimal _currentValue = 7.5m;
        public override decimal CurrentValue { get { return _currentValue; } }

        public TechStock(string marketID, Texture2D icon)
            : base(marketID, icon)
        {
        }

        public override void Purchase(decimal quantity)
        {
            throw new NotImplementedException();
        }
    }
}
