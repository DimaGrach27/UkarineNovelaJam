using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionTestDebug : IActionScreen
    {
        public void Action()
        {
            Debug.Log("Debug action!");
        }

        public ActionType ActionType => ActionType.DEBUG;
    }
}