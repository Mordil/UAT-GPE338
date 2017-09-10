using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
    [Serializable]
    public class StoreItemModel : IJsonModel
    {
        public delegate void ImageDownloadedHandler();

        public ImageDownloadedHandler ImageDownloaded;

        public string title;
        public string thumbnailUrl;
        public Texture2D thumbnailImage;

        public static StoreItemModel CreateFromJSON(string json)
        {
            return JsonUtility.FromJson<StoreItemModel>(json);
        }

        public IEnumerator FetchImageData()
        {
            WWW request = new WWW(thumbnailUrl + ".png");

            yield return request;

            thumbnailImage = request.texture;

            ImageDownloaded();
        }
    }
}
