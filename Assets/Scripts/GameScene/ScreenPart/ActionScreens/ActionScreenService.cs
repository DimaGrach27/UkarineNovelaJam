using System.Collections.Generic;
using GameScene.Health;
using GameScene.ScreenPart.ActionScreens.Actions;
using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens
{
    public class ActionScreenService
    {
        private readonly Dictionary<ActionType, IActionScreen> _actionMap = new();

        public ActionScreenService(HealthService healthService, Transform ui)
        {
            _actionMap.Add(ActionType.DEBUG, new ActionTestDebug());
            _actionMap.Add(ActionType.CAMERA_SHAKER, new ActionCameraShaker());
            
            _actionMap.Add(ActionType.HEAL_ONE_HEALTH, new ActionHealOneHealth(healthService));
            _actionMap.Add(ActionType.HEAL_FULL_HEALTH, new ActionFullHeal(healthService));
            _actionMap.Add(ActionType.DAMAGE_ONE_HEALTH, new ActionDamageOneHealth(healthService));
            
            
            _actionMap.Add(ActionType.ALL_ITEM_WAS_FOUND, new ActionAllItemWasFound(ui));
        }

        public void Action(ActionType actionType)
        {
            if(!_actionMap.ContainsKey(actionType)) return;
            
            _actionMap[actionType].Action();
        }
    }
}