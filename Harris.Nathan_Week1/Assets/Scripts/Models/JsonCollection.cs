using Assets.Scripts.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
    public class JsonCollection<T>
        where T : IJsonModel
    {
        public T[] items;

        public static JsonCollection<T> CreateFromJSON(string json, int maxItems = 0, bool wrapRootNode = false)
        {
            if (wrapRootNode)
            {
                json = "{ \"items\": " + json + " }";
            }
            return CreateFromJSON(json, maxItems);
        }

        private static JsonCollection<T> CreateFromJSON(string json, int maxItems)
        {
            var result = JsonUtility.FromJson<JsonCollection<T>>(json);
            
            if (maxItems > 0)
            {
                result.items = result.items
                    .Take(maxItems)
                    .ToArray();
            }

            return result;
        }
    }
}
