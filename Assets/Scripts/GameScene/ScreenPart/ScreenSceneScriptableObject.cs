using System;
using GameScene.BgScreen;
using GameScene.Characters;
using GameScene.ScreenPart.ActionScreens;
using UnityEngine;

namespace GameScene.ScreenPart
{
    [CreateAssetMenu(fileName = "Screen Scene", menuName = "Frog Croaked Team/Create 'Screen Scene'", order = 0)]
    public class ScreenSceneScriptableObject : ScriptableObject
    {
        [SerializeField] private string sceneKey;
        
        [SerializeField] private BgEnum bgEnum;
        [SerializeField] private ActionType actionType;
        
        [SerializeField] private NextScene[] nextScenes;
        [SerializeField] private ScreenPart[] screenParts;
        
        public BgEnum Bg => bgEnum;
        public ActionType ActionType => actionType;
        
        public string SceneKey => sceneKey;

        public ScreenPart[] ScreenParts => screenParts;
        public NextScene[] NextScenes => nextScenes;
    }

    [Serializable]
    public class ScreenPart
    {
        [SerializeField] private ActionType actionType;
        [SerializeField] private CharacterScreenPositionEnum screenPosition;
        
        [SerializeField] private Sprite characterImage;
        
        [SerializeField] private string nameCharacter;
        [SerializeField, TextArea(1, 4)] private string textShow;
        
        public string CharacterName => nameCharacter;
        public string TextShow => textShow;
        
        public Sprite Image => characterImage;
        
        public CharacterScreenPositionEnum Position => screenPosition;
        public ActionType ActionType => actionType;
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
}