using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public static class ExtensionMethods
    {
        public static void ScrollByMouseWheel(this Scrollbar scrollbar)
        {
            float scrollStrange = 3.0f;
            float mouseScroll = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            float scrollbarValue = scrollbar.value;
            
            scrollbar.value = Mathf.Clamp01(scrollbarValue + mouseScroll * scrollStrange);
        }
    }
}