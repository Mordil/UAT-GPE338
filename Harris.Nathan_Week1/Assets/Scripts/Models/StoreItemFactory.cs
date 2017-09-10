using Assets.Scripts.Interfaces;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Models
{
    static class StoreItemFactory
    {
        public delegate void CompletionHandler(GameObject[] items);

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

        public static CompletionHandler OnTaskCompleted;
        
        private static Thread _backgroundThread;
        private static object _lockObj = new object();

        static StoreItemFactory()
        {
            OnTaskCompleted += items => _backgroundThread = null;
        }

        public static void TransformJsonToModel(string json, GameObject prefab)
        {
            if (_backgroundThread != null)
            {
                _backgroundThread.Abort();
                _backgroundThread = null;
            }

            _backgroundThread = new Thread(new ParameterizedThreadStart(TransformData<StoreItemModel, StoreItem>));
            _backgroundThread.Start(new ThreadParams(json, prefab));
        }

        private static void TransformData<T, U>(object threadParams)
            where T : IJsonModel
            where U : IModelBacked<T>
        {
            lock(_lockObj)
            {
                var parameters = threadParams as ThreadParams;
                var data = JsonCollection<T>.CreateFromJSON(parameters.JSON, 10, true);

                TaskManager.Enqueue(() =>
                {
                    OnTaskCompleted(GeneratePrefabs<T, U>(data.items, parameters.Prefab));
                });
            }
        }

        private static GameObject[] GeneratePrefabs<T, U>(T[] models, GameObject prefab)
            where T : IJsonModel
            where U : IModelBacked<T>
        {
            var result = new GameObject[models.Length];

            for (int i = 0; i < models.Length; i++)
            {
                var newObj = Object.Instantiate(prefab) as GameObject;
                newObj.name = "StoreItem_" + (i + 1);
                var script = newObj.GetComponent<U>();
                script.Model = models[i];
                result[i] = newObj;
            }

            return result;
        }
    }
}
