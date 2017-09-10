using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class TaskManager
    {
        private static object _lockObj = new object();
        private static bool shouldContinue;
        private static Queue<Action> _queue;

        static TaskManager()
        {
            _queue = new Queue<Action>();
        }

        public static IEnumerator StartCycle()
        {
            shouldContinue = true;

            while(shouldContinue)
            {
                if (_queue.Count > 0)
                {
                    lock(_lockObj)
                    {
                        foreach (Action action in _queue)
                        {
                            action();
                        }

                        _queue.Clear();
                    }
                }

                yield return null;
            }
        }

        public static void StopCycle()
        {
            shouldContinue = false;
        }

        public static void Enqueue(Action action)
        {
            lock(_lockObj)
            {
                _queue.Enqueue(action);
            }
        }
    }
}
