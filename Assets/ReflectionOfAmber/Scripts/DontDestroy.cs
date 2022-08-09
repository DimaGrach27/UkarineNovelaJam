using UnityEngine;

namespace ReflectionOfAmber.Scripts
{
    public class DontDestroy : MonoBehaviour
    {
        private static bool _wasInit;

        private void Awake()
        {
            if (_wasInit) return;

            _wasInit = true;
            if (!GameModel.GameWasInit)
                DontDestroyOnLoad(this);
            else
            {
                Destroy(gameObject);
            }
        }
    }
}