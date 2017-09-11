using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// A Plain Old C# Object data model that stores data to be used by a MonoBehavior.
    /// </summary>
    [Serializable]
    public class StoreItemModel : IJsonModel
    {
        /// <summary>
        /// Callback handler for when the image has been populated.
        /// </summary>
        public delegate void ImageDownloadedHandler();

        /// <summary>
        /// Event triggered when the image download has been requested and completed.
        /// </summary>
        public ImageDownloadedHandler ImageDownloaded;
        
        public string title;
        public string thumbnailUrl;
        public Texture2D thumbnailImage;

        /// <summary>
        /// Creates a new StoreItemModel from the provided JSON.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static StoreItemModel CreateFromJSON(string json)
        {
            return JsonUtility.FromJson<StoreItemModel>(json);
        }

        /// <summary>
        /// Downloads the image from the URL and stores it in a local reference before invoking the ImageDownloaded event.
        /// </summary>
        /// <returns></returns>
        public IEnumerator FetchImageData()
        {
            // Documentation specifies that the image needs to be either PNG or JPEG, so we explicitly add it here
            // because the API I'm using doesn't
            WWW request = new WWW(thumbnailUrl + ".png");

            yield return request;

            thumbnailImage = request.texture;

            ImageDownloaded();
        }
    }
}
