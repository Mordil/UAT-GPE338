using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    public static class TaskManager
    {
        private static bool shouldContinue;
        private static List<Action> _queue;

        static TaskManager()
        {
            _queue = new List<Action>();
        }

        public static IEnumerator StartCycle()
        {
            shouldContinue = true;

            while(shouldContinue)
            {
                if (_queue.Count > 0)
                {
                    foreach (Action action in _queue)
                    {
                        action();
                    }

                    _queue.Clear();
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
            _queue.Add(action);
        }
    }
}
