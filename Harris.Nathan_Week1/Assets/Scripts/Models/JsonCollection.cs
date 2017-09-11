using Assets.Scripts.Interfaces;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// A generic class that is JsonUtility serializable for JSON arrays.
    /// </summary>
    /// <typeparam name="T">The root model that will be serialized from the JSON data.</typeparam>
    public class JsonCollection<T>
        where T : IJsonModel
    {
        /// <summary>
        /// The collection of items that have been deserialized from JSON.
        /// </summary>
        public T[] items;

        /// <summary>
        /// Deserializes the json into the desired sub-model.
        /// </summary>
        /// <param name="json">The JSON to parse</param>
        /// <param name="maxItems">The max number of items to retain. If 0, all items from the JSON string will be retained.</param>
        /// <param name="wrapRootNode">Should the root JSON node be wrapped in an object? JsonUtility requires a root node in order to function.</param>
        /// <returns></returns>
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
