using UnityEngine;

namespace Assets.Scripts
{
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
            StartCoroutine(TaskManager.StartCycle());
        }
    }
}
