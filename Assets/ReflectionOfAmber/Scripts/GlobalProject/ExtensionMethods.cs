using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GlobalProject
{
    public static class ExtensionMethods
    {
        private static InputAction MouseScrollInputAction;
        private static InputAction NavigationScrollInputAction;
        
        public static void ScrollByMouseWheel(this Scrollbar scrollbar)
        {
            MouseScrollInputAction ??= InputSystem.actions.FindAction("ScrollWheel");
            NavigationScrollInputAction ??= InputSystem.actions.FindAction("Navigate");
            
            float scrollStrange = 0.3f;
            float mouseScroll = MouseScrollInputAction.ReadValue<Vector2>().y;
            float navigationScroll = NavigationScrollInputAction.ReadValue<Vector2>().y;
            bool navigateWasPerformed = NavigationScrollInputAction.WasPressedThisDynamicUpdate();
            
            navigationScroll = navigateWasPerformed ? navigationScroll : 0.0f;
            
            // Debug.Log($"MOUSE SCROLL: {mouseScroll}  NAVIGATE SCROLL: {navigationScroll}");
            
            float scrollbarValue = scrollbar.value;
            
            scrollbar.value = Mathf.Clamp01(scrollbarValue + (mouseScroll + navigationScroll) * scrollStrange);
        }
    }
}