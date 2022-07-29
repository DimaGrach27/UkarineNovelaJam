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
            ScreenPartsService screenPartsService, 
            Transform ui)
        {
            _actionMap.Add(ActionType.DEBUG, new ActionTestDebug());
            _actionMap.Add(ActionType.CAMERA_SHAKER, new ActionCameraShaker());
            _actionMap.Add(ActionType.CAMERA_SHAKER_LONG, new ActionCameraShakerLong());
            
            _actionMap.Add(ActionType.HEAL_ONE_HEALTH, new ActionHealOneHealth(healthService));
            _actionMap.Add(ActionType.HEAL_FULL_HEALTH, new ActionFullHeal(healthService));
            _actionMap.Add(ActionType.DAMAGE_ONE_HEALTH, new ActionDamageOneHealth(healthService, this));
            _actionMap.Add(ActionType.TAKE_PHOTO, new ActionTakePhoto(cameraActionService));
            _actionMap.Add(ActionType.RADIO_CHANGE, new ActionRadioChange());
            _actionMap.Add(ActionType.FADE_OUT_IN, new ActionFadeOutIn());
            _actionMap.Add(ActionType.WATER_SLAP_SOUND, new ActionWaterSlap());
            _actionMap.Add(ActionType.METAL_DRAG_SOUND, new ActionDragMetal());
            _actionMap.Add(ActionType.PLAY_CRASH_SOUND, new ActionCrashSound(this));
            _actionMap.Add(ActionType.FALLING_TREE, new ActionFallenTree(this));
            
            _actionMap.Add(ActionType.FADE_OUT_MUSIC_ON_LOPPER, new ActionFadeOutMusicOnLooper());
            _actionMap.Add(ActionType.FADE_IN_MUSIC_ON_LOPPER, new ActionFadeInMusicOnLooper());
            _actionMap.Add(ActionType.STOP_MUSIC_ON_LOPPER, new ActionStopMusicOnLooper());
            _actionMap.Add(ActionType.STOP_ALL_MUSIC, new ActionStopAllMusic());
            _actionMap.Add(ActionType.PLAY_DOG_BARK_SOUND_ON_LOPPER, new ActionBarkLoop());
            _actionMap.Add(ActionType.PLAY_HEART_BEEP_SOUND_ON_LOPPER, new ActionHeartBeepLoop());
            _actionMap.Add(ActionType.PLAY_BEEP_SOUND_ON_LOPPER, new ActionBeepLoop());
            _actionMap.Add(ActionType.PLAY_EMBIENT_SLOW, new ActionEmbientSlow());
            _actionMap.Add(ActionType.PLAY_EMBIENT_FAST, new ActionEmbientFast());
            
            _actionMap.Add(ActionType.ADD_EVIDENCE_ILONA, new ActionAddEvidenceForIlona());
            _actionMap.Add(ActionType.ADD_EVIDENCE_ZAHARES, new ActionAddEvidenceForZahares());
            _actionMap.Add(ActionType.ADD_EVIDENCE_OLEKSII, new ActionAddEvidenceForOleksii());
            
            _actionMap.Add(ActionType.OPEN_EYE_ANIMA, new ActionOpenEye(screenPartsService));
            
            
            _actionMap.Add(ActionType.ALL_ITEM_WAS_FOUND, new ActionAllItemWasFound(ui));
        }
        

        public void Action(ActionType actionType)
        {
            if(!_actionMap.ContainsKey(actionType)) return;
            
            _actionMap[actionType].Action();
        }
    }
}