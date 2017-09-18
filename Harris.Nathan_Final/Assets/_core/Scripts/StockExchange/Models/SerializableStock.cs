using System;

namespace SE.Models
{
    [Serializable]
    public class SerializableStock<T>
        where T : IStock
    {
        public string type;
        public string marketId;
        public decimal startingValue;
    }
}
