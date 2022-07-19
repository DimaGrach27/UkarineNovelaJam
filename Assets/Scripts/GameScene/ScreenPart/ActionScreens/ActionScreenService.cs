using System.Collections.Generic;
using GameScene.ScreenPart.ActionScreens.Actions;

namespace GameScene.ScreenPart.ActionScreens
{
    public class ActionScreenService
    {
        private readonly Dictionary<ActionType, IActionScreen> _actionMap = new();

        public ActionScreenService()
        {
            _actionMap.Add(ActionType.DEBUG, new ActionTestDebug());
            _actionMap.Add(ActionType.CAMERA_SHAKER, new ActionCameraShaker());
        }

        public void Action(ActionType actionType)
        {
            if(!_actionMap.ContainsKey(actionType)) return;
            
            _actionMap[actionType].Action();
        }
    }
}