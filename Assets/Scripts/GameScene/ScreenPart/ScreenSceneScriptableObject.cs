using System;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ScreenPart.ActionScreens;
using GameScene.ScreenPart.SpecialSO;
using UnityEngine;

namespace GameScene.ScreenPart
{
    [CreateAssetMenu(
        fileName = "Screen Scene", 
        menuName = "Frog Croaked Team/Create 'Screen Scene'", 
        order = 0)]
    
    public class ScreenSceneScriptableObject : ScriptableObject
    {
        [SerializeField] private string sceneKey;
        
        [SerializeField] private BgEnum bgEnum;
        
        [SerializeField] private bool isActiveCamera = false;

        
        [SerializeField] private ActionType[] actionsType;
        
        [SerializeField] private StatusSetter statusSetter;

        [SerializeField] private ScreenPart[] screenParts;
        [SerializeField] private NextScene[] nextScenes;
        
        public BgEnum Bg => bgEnum;
        public ActionType[] ActionsType => actionsType;

        public StatusSetter StatusSetter => statusSetter;
        
        public string SceneKey => sceneKey;
        
        public bool IsActiveCamera => isActiveCamera;

        public ScreenPart[] ScreenParts => screenParts;
        public NextScene[] NextScenes => nextScenes;
    }

    [Serializable]
    public class ScreenPart
    {
        [SerializeField] private ActionType[] actionsType;
        [SerializeField] private StatusSetter statusSetter;
        [SerializeField] private CharacterScreenPositionEnum screenPosition;
        
        [SerializeField] private Sprite characterImage;
        
        [SerializeField] private string nameCharacter;
        [SerializeField, TextArea(1, 4)] private string textShow;
        
        public string CharacterName => nameCharacter;
        public string TextShow => textShow;
        
        public Sprite Image => characterImage;
        
        public StatusSetter StatusSetter => statusSetter;
        
        public CharacterScreenPositionEnum Position => screenPosition;
        public ActionType[] ActionsType => actionsType;
    }
    
    [Serializable]
    public class NextScene
    {
        [SerializeField] private ScreenSceneScriptableObject scene;
        [SerializeField, TextArea(1, 4)] private string chooseText;

        public StatusDependent statusDependent;
        public CameraDependent cameraDependent;
        public ExclusionDependent exclusionDependent;
        public FindDependent findDependent;

        public string ChooseText => chooseText;
        public ScreenSceneScriptableObject Scene => scene;
    }

    [Serializable]
    public class StatusDependent
    {
        public bool enable = false;
        public bool value;
        public StatusEnum status = StatusEnum.NONE;
    }
    
    [Serializable]
    public class CameraDependent
    {
        public bool enable;
        public bool visibleOnPhoto;
        public bool visibleOutPhoto;
    }
    
    [Serializable]
    public class ExclusionDependent
    {
        public bool enable;
    }
    
    [Serializable]
    public class FindDependent
    {
        public bool enable;
        public bool value = true;
        public StatusEnum status = StatusEnum.NONE;
    }
    
    [Serializable]
    public class SpecialDependent
    {
        public bool enable;
        public SpecialScriptableObjectBase special;
    }
    
    [Serializable]
    public class StatusSetter
    {
        [SerializeField] private bool enable = false;
        [SerializeField] private bool statusFlag = true;
        [SerializeField] private StatusEnum status = StatusEnum.NONE;

        public bool Enable => enable;
        public bool StatusFlag => statusFlag;
        public StatusEnum Status => status;
    }
}