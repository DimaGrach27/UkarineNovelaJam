using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public static class EventSystemUtility
    {
        private static InputSystemUIInputModule s_Module;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Init()
        {
            s_Module = null;
        }

        public static bool IsPointerOverGUIAction()
        {
            if (!EventSystem.current)
            {
                return false;
            }

            if (!s_Module)
            {
                s_Module = (InputSystemUIInputModule)EventSystem.current.currentInputModule;
            }

            return s_Module.GetLastRaycastResult(Pointer.current.deviceId).isValid;
        }
    }
}