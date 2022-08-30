using UnityEngine;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugBritness : MonoBehaviour
    {
        public float britness = 1;

        private void Update()
        {
            Screen.brightness = britness;
        }
    }
}