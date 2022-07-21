using System;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ScreenPart.ActionScreens;
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
        [SerializeField] private ActionType[] actionsType;
        
        [SerializeField] private StatusDependent statusDependent;
        [SerializeField] private StatusSetter statusSetter;

        [SerializeField] private bool isActiveCamera = false;
        [SerializeField] private NextScene[] nextScenes;
        [SerializeField] private ScreenPart[] screenParts;
        
        public BgEnum Bg => bgEnum;
        public ActionType[] ActionsType => actionsType;
        public StatusDependent StatusDependent => statusDependent;
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
        [SerializeField] private CharacterScreenPositionEnum screenPosition;
        
        [SerializeField] private Sprite characterImage;
        
        [SerializeField] private string nameCharacter;
        [SerializeField, TextArea(1, 4)] private string textShow;
        
        public string CharacterName => nameCharacter;
        public string TextShow => textShow;
        
        public Sprite Image => characterImage;
        
        public CharacterScreenPositionEnum Position => screenPosition;
        public ActionType[] ActionsType => actionsType;
    }
    
    [Serializable]
    public class NextScene
    {
        [SerializeField] private ScreenSceneScriptableObject scene;
        
        [SerializeField, TextArea(1, 4)] private string chooseText;
        
        [SerializeField] private bool isShowOnCameraAction = true;
        [SerializeField] private bool isReadyToShow = true;

        public bool IsShowOnCameraAction => isShowOnCameraAction;
        public bool IsReadyToShow => isReadyToShow;
        
        public string ChooseText => chooseText;
        public ScreenSceneScriptableObject Scene => scene;

    }

    [Serializable]
    public class StatusDependent
    {
        [SerializeField] private bool enable = false;
        [SerializeField] private StatusEnum status = StatusEnum.NONE;

        public bool Enable => enable;
        public StatusEnum Status => status;
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