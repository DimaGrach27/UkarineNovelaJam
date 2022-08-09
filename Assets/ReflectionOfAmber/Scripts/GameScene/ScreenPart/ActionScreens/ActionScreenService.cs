using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction;
using ReflectionOfAmber.Scripts.GameScene.GlobalVolume;
using ReflectionOfAmber.Scripts.GameScene.Health;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions;
using ReflectionOfAmber.Scripts.GameScene.Services;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens
{
    public class ActionScreenService
    {
        private readonly Dictionary<ActionType, ActionBase> _actionMap = new();

        public readonly HealthService HealthService;
        public readonly CameraActionService CameraActionService;
        public readonly BgService BgService;
        public readonly GamePlayCanvas GamePlayCanvas;
        public readonly ScreenPartsServiceFacade ScreenPartsServiceFacade;
        public readonly AudioSystemService AudioSystemService;
        public readonly GlobalVolumeService GlobalVolumeService;
        public readonly OpenEyeAnimation OpenEyeAnimation;
        public readonly CoroutineHelper CoroutineHelper;

        [Inject]
        public ActionScreenService(
            HealthService healthService, 
            CameraActionService cameraActionService,
            BgService bgService,
            GamePlayCanvas gamePlayCanvas,
            ScreenPartsServiceFacade screenPartsServiceFacade,
            AudioSystemService audioSystemService,
            GlobalVolumeService globalVolumeService,
            OpenEyeAnimation openEyeAnimation,
            CoroutineHelper coroutineHelper
            )
        {
            HealthService = healthService;
            CameraActionService = cameraActionService;
            BgService = bgService;
            GamePlayCanvas = gamePlayCanvas;
            ScreenPartsServiceFacade = screenPartsServiceFacade;
            AudioSystemService = audioSystemService;
            GlobalVolumeService = globalVolumeService;
            OpenEyeAnimation = openEyeAnimation;
            CoroutineHelper = coroutineHelper;
            
            _actionMap.Add(ActionType.DEBUG, new ActionTestDebug());
            _actionMap.Add(ActionType.CAMERA_SHAKER, new ActionCameraShaker());
            _actionMap.Add(ActionType.CAMERA_SHAKER_LONG, new ActionCameraShakerLong());
            
            _actionMap.Add(ActionType.HEAL_ONE_HEALTH, new ActionHealOneHealth());
            _actionMap.Add(ActionType.HEAL_FULL_HEALTH, new ActionFullHeal());
            _actionMap.Add(ActionType.DAMAGE_ONE_HEALTH, new ActionDamageOneHealth());
            _actionMap.Add(ActionType.TAKE_PHOTO, new ActionTakePhoto());
            _actionMap.Add(ActionType.RADIO_CHANGE, new ActionRadioChange());
            _actionMap.Add(ActionType.FADE_OUT_IN, new ActionFadeOutIn());
            _actionMap.Add(ActionType.WATER_SLAP_SOUND, new ActionWaterSlap());
            _actionMap.Add(ActionType.METAL_DRAG_SOUND, new ActionDragMetal());
            _actionMap.Add(ActionType.PLAY_CRASH_SOUND, new ActionCrashSound());
            _actionMap.Add(ActionType.FALLING_TREE, new ActionFallenTree());
            
            _actionMap.Add(ActionType.FADE_OUT_MUSIC_ON_LOPPER, new ActionFadeOutMusicOnLooper());
            _actionMap.Add(ActionType.FADE_IN_MUSIC_ON_LOPPER, new ActionFadeInMusicOnLooper());
            _actionMap.Add(ActionType.STOP_MUSIC_ON_LOPPER, new ActionStopMusicOnLooper());
            _actionMap.Add(ActionType.STOP_ALL_MUSIC, new ActionStopAllMusic());
            _actionMap.Add(ActionType.PLAY_DOG_BARK_SOUND_ON_LOPPER, new ActionBarkLoop());
            _actionMap.Add(ActionType.PLAY_HEART_BEEP_SOUND_ON_LOPPER, new ActionHeartBeepLoop());
            _actionMap.Add(ActionType.PLAY_BEEP_SOUND_ON_LOPPER, new ActionBeepLoop());
            _actionMap.Add(ActionType.PLAY_EMBIENT_SLOW, new ActionEmbientSlow());
            _actionMap.Add(ActionType.PLAY_EMBIENT_FAST, new ActionEmbientFast());
            
            _actionMap.Add(ActionType.START_ABERRATION_LOOP, new ActionStartAberrationLoop());
            _actionMap.Add(ActionType.STOP_ABERRATION_LOOP, new ActionStopAberrationLoop());
            
            _actionMap.Add(ActionType.ADD_EVIDENCE_ILONA, new ActionAddEvidenceForIlona());
            _actionMap.Add(ActionType.ADD_EVIDENCE_ZAHARES, new ActionAddEvidenceForZahares());
            _actionMap.Add(ActionType.ADD_EVIDENCE_OLEKSII, new ActionAddEvidenceForOleksii());
            
            _actionMap.Add(ActionType.CHANGE_GLITCH_SCREEN, new ActionChangeScreenGlitch());
            _actionMap.Add(ActionType.CHANGE_DARK_SCREEN, new ActionChangeScreenDark());
            _actionMap.Add(ActionType.CHANGE_FOREST_SCREEN, new ActionChangeScreenForest());
            
            _actionMap.Add(ActionType.OPEN_EYE_ANIMA, new ActionOpenEye());
            _actionMap.Add(ActionType.PREPARE_OPEN_EYE_ANIMA, new ActionPrepareOpenEye());
            
            _actionMap.Add(ActionType.ALL_ITEM_WAS_FOUND, new ActionAllItemWasFound());

            foreach (var actionBase in _actionMap.Values)
            {
                actionBase.InitService(this);
            }
        }
        
        public void Action(ActionType actionType)
        {
            if(!_actionMap.ContainsKey(actionType)) return;
            
            _actionMap[actionType].Action();
        }
    }
}