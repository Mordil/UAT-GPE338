using Assets.Scripts.Interfaces;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// Factory class for generating StoreItems by making web requests in a background thread.
    /// </summary>
    static class StoreItemFactory
    {
        /// <summary>
        /// Completion handler for when a background thread task has completed.
        /// </summary>
        /// <param name="items">The prepared game objects from a prefab.</param>
        public delegate void CompletionHandler(GameObject[] items);

        /// <summary>
        /// Container object for passing multiple objects to the background thread.
        /// </summary>
        private class ThreadParams
        {
            public string JSON { get; private set; }
            public GameObject Prefab { get; private set; }

            public ThreadParams(string json, GameObject prefab)
            {
                JSON = json;
                Prefab = prefab;
            }
        }

        /// <summary>
        /// Event fired when the background thread and main thread tasks are completed.
        /// </summary>
        public static CompletionHandler OnTaskCompleted;
        
        private static Thread _backgroundThread;

        static StoreItemFactory()
        {
            // Always null out the thread when the job is done to ensure cleanup
            OnTaskCompleted += items => _backgroundThread = null;
        }

        /// <summary>
        /// Triggers a new thread to be created that will make URL requests and deserialize JSON into models.
        /// </summary>
        /// <param name="json"></param>
        /// <param name="prefab"></param>
        public static void TransformJsonToModel(string json, GameObject prefab)
        {
            // if there's already a thread running, we want to abort and clean up before starting a new one
            if (_backgroundThread != null)
            {
                _backgroundThread.Abort();
                _backgroundThread = null;
            }

            // create a thread with parameters brought from the caller
            // Since this is a StoreItem factory, we can hardcode the model & monobehaviour script we're creating
            _backgroundThread = new Thread(new ParameterizedThreadStart(TransformData<StoreItemModel, StoreItem>));
            _backgroundThread.Start(new ThreadParams(json, prefab));
        }

        /// <summary>
        /// Deserializes the JSON and then creates the GameObjects before invoking the OnTaskCompleted event.
        /// </summary>
        /// <typeparam name="T">The JSON Model being deserialized to.</typeparam>
        /// <typeparam name="U">The MonoBehaviour script waiting for the deserialized model.</typeparam>
        /// <param name="threadParams"></param>
        private static void TransformData<T, U>(object threadParams)
            where T : IJsonModel
            where U : IModelBacked<T>
        {
            var parameters = threadParams as ThreadParams;
            var data = JsonCollection<T>.CreateFromJSON(parameters.JSON, 10, true);

            // Since Unity APIs are only available on the main thread, do the remaining work on the main thread with the TaskManager
            // The only work that should be done on this thread is Unity APIs - all other intensive work should be done before this point
            TaskManager.Enqueue(() =>
            {
                OnTaskCompleted(GeneratePrefabs<T, U>(data.items, parameters.Prefab));
            });
        }

        /// <summary>
        /// Creates the desired prefabs and assigns the ModelBacked component with the appropriate model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="models"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        private static GameObject[] GeneratePrefabs<T, U>(T[] models, GameObject prefab)
            where T : IJsonModel
            where U : IModelBacked<T>
        {
            // pre-initialize with the known size of the array
            var result = new GameObject[models.Length];

            for (int i = 0; i < models.Length; i++)
            {
                // create the new object
                var newObj = Object.Instantiate(prefab) as GameObject;
                // give it a nice name in the editor
                newObj.name = "StoreItem_" + (i + 1);
                // get the script and assign it its model
                // since it's constrained to IModelBacked, we know it'll have a "Model" property
                var script = newObj.GetComponent<U>();
                script.Model = models[i];
                // assign the new GameObject to the same position in the result array that will be returned to the original stack caller
                result[i] = newObj;
            }

            return result;
        }
    }
}
