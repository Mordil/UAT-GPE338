using System;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts
{
    /// <summary>
    /// Static class for invoking methods on the main thread, such as Unity APIs.
    /// This allows background threads to access Unity APIs or to schedule tasks more discretely.
    /// </summary>
    public static class TaskManager
    {
        private static object _lockObj = new object();
        private static bool shouldContinue;
        private static Queue<Action> _queue;

        static TaskManager()
        {
            // initialize the queue to use later
            _queue = new Queue<Action>();
        }

        /// <summary>
        /// Starts a coroutine for invoking all queued actions every frame before clearing the queue.
        /// </summary>
        /// <returns></returns>
        public static IEnumerator StartCycle()
        {
            shouldContinue = true;

            while(shouldContinue)
            {
                // only need to do work if there are items in the queue
                if (_queue.Count > 0)
                {
                    // since we're using and editing the queue, lock access
                    lock(_lockObj)
                    {
                        // invoke all the actions in order
                        foreach (Action action in _queue)
                        {
                            action();
                        }

                        // all actions are done, clear the queue
                        _queue.Clear();
                    }
                }

                // keep continuing until flag is disabled
                yield return null;
            }
        }

        /// <summary>
        /// Stops the frame ticks and queue invocations.
        /// </summary>
        public static void StopCycle()
        {
            shouldContinue = false;
        }

        /// <summary>
        /// Adds the added action to the queue to be invoked on the next frame tick.
        /// </summary>
        /// <param name="action"></param>
        public static void Enqueue(Action action)
        {
            // lock to ensure that actions are getting added in order
            // I don't know the timing or which thread might be making a call to this method
            // so it needs a lock while changing the underlying array
            lock(_lockObj)
            {
                _queue.Enqueue(action);
            }
        }
    }
}
