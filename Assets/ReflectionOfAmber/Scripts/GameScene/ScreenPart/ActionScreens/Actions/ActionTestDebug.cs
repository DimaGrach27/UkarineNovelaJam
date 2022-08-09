using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionTestDebug : ActionBase
    {
        public override void Action()
        {
            Debug.Log("Debug action!");
        }

        public override ActionType ActionType => ActionType.DEBUG;
    }
}