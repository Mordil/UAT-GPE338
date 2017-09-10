using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class StoreItem
    {
        public string title;
        public string thumbnailUrl;
        public Texture2D thumbnailImage;

        private object _lockObj;

        public static StoreItem CreateFromJSON(string json)
        {
            return JsonUtility.FromJson<StoreItem>(json);
        }

        public IEnumerator FetchImageData()
        {
            WWW request = new WWW(thumbnailUrl);

            yield return request;

            lock(_lockObj)
            {
                thumbnailImage = request.texture;
            }
        }
    }
}
