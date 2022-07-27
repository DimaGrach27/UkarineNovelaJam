using System.Collections.Generic;
using GameScene.ChooseWindow.CameraAction;
using GameScene.Health;
using GameScene.ScreenPart.ActionScreens.Actions;
using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens
{
    public class ActionScreenService
    {
        private readonly Dictionary<ActionType, IActionScreen> _actionMap = new();

        public ActionScreenService(
            HealthService healthService, 
            CameraActionService cameraActionService, 
            Transform ui)
        {
            _actionMap.Add(ActionType.DEBUG, new ActionTestDebug());
            _actionMap.Add(ActionType.CAMERA_SHAKER, new ActionCameraShaker());
            
            _actionMap.Add(ActionType.HEAL_ONE_HEALTH, new ActionHealOneHealth(healthService));
            _actionMap.Add(ActionType.HEAL_FULL_HEALTH, new ActionFullHeal(healthService));
            _actionMap.Add(ActionType.DAMAGE_ONE_HEALTH, new ActionDamageOneHealth(healthService));
            _actionMap.Add(ActionType.TAKE_PHOTO, new ActionTakePhoto(cameraActionService));
            _actionMap.Add(ActionType.RADIO_CHANGE, new ActionRadioChange());
            _actionMap.Add(ActionType.FADE_OUT_IN, new ActionFadeOutIn());
            _actionMap.Add(ActionType.PLAY_CRASH_SOUND, new ActionCrashSound());
            
            
            _actionMap.Add(ActionType.ALL_ITEM_WAS_FOUND, new ActionAllItemWasFound(ui));
        }

        public void Action(ActionType actionType)
        {
            if(!_actionMap.ContainsKey(actionType)) return;
            
            _actionMap[actionType].Action();
        }
    }
}