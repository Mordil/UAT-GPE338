using Assets.Scripts.Models;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    static class StoreItemFactory
    {
        public delegate void CompletionHandler(GameObject[] items);

        public static CompletionHandler OnTaskCompleted;
        
        private static Thread _backgroundThread;
        private static object _lockObj = new object();

        static StoreItemFactory()
        {
            OnTaskCompleted += items => _backgroundThread = null;
        }

        public static void TransformJsonToModel(string json)
        {
            _backgroundThread = new Thread(new ParameterizedThreadStart(TransformData));
            _backgroundThread.Start(json);
        }

        private static void TransformData(object json)
        {
            lock(_lockObj)
            {
                TaskManager.Enqueue(() =>
                {
                    GameObject[] result = new GameObject[0];
                    OnTaskCompleted(result);
                });
            }
        }
    }
}
