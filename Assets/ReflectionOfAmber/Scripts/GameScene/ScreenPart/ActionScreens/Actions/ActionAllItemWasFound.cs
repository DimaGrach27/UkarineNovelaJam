using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAllItemWasFound : IActionScreen
    {
        public ActionAllItemWasFound(Transform ui)
        {
            _allItemWasFoundUiView = ui.GetComponentInChildren<AllItemWasFoundUiView>();
        }
        
        private readonly AllItemWasFoundUiView _allItemWasFoundUiView;
        
        public void Action()
        {
            _allItemWasFoundUiView.Show();
        }

        public ActionType ActionType => ActionType.ALL_ITEM_WAS_FOUND;
    }
}