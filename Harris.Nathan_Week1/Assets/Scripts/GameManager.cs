using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Master singleton manager script that maintains app-wide scripts and data.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this);
            // Start handling the TaskManager for main thread access across the app.
            StartCoroutine(TaskManager.StartCycle());
        }
    }
}
