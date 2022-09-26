using System;
using ReflectionOfAmber.Scripts.Characters;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.Characters;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart
{
    [CreateAssetMenu(
        fileName = "Screen Scene", 
        menuName = "Frog Croaked Team/Create 'Screen Scene'", 
        order = 0)]
    
    public class ScreenSceneScriptableObject : ScriptableObject
    {
        [SerializeField] private string sceneKey;
        
        [SerializeField] private bool isActiveCamera = false;
        
        [SerializeField, Toggle("enable")] private ChangeBackGround changeBackGround;
        [SerializeField, Toggle("enable")] private StatusSetter statusSetter;
        [SerializeField, Toggle("enable")] private CountSetter countSetter;
        
        [SerializeField] private ActionType[] actionsType;

        [SerializeField] private ScreenPart[] screenParts;
        [SerializeField] private NextScene[] nextScenes;
        
        public ChangeBackGround ChangeBackGround => changeBackGround;
        public ActionType[] ActionsType => actionsType;

        public StatusSetter StatusSetter => statusSetter;
        public CountSetter CountSetter => countSetter;
        
        public string SceneKey => sceneKey;
        
        public bool IsActiveCamera => isActiveCamera;

        public ScreenPart[] ScreenParts => screenParts;
        public NextScene[] NextScenes => nextScenes;
    }

    [Serializable]
    public class ScreenPart
    {
        [SerializeField] private ActionType[] actionsType;
        [SerializeField] private MusicType musicTypeOnStart = MusicType.NONE;
        [SerializeField, Toggle("enable")] private StatusSetter statusSetter;
        
        [SerializeField, EnumPaging] private CharacterScreenPositionEnum screenPosition;
        [SerializeField, EnumPaging] private CharacterSprite characterImage;
        [SerializeField, EnumPaging] private CharacterName nameCharacter;
        
        [SerializeField, TextArea(1, 6)] private string textShow;
        [SerializeField] private bool endOfText = true;
        [SerializeField] private ActionType[] actionsTypeEnd;
        
        public string CharacterName => TranslatorParser.GetText(nameCharacter.ToString());
        public string CharacterNameType => nameCharacter.ToString();
        public string TextShow => textShow;
        
        public Sprite Image => CharactersService.GetSprite(characterImage);
        
        public StatusSetter StatusSetter => statusSetter;
        
        public CharacterScreenPositionEnum Position => screenPosition;
        public ActionType[] ActionsType => actionsType;
        public ActionType[] ActionsTypeEnd => actionsTypeEnd;
        public MusicType MusicTypeOnStart => musicTypeOnStart;
        public bool EndOfText => endOfText;
    }
    
    [Serializable]
    public class NextScene
    {
        [SerializeField] private ScreenSceneScriptableObject scene;
        [SerializeField, TextArea(1, 4)] private string chooseText;

        [Toggle("enable")] public StatusDependent statusDependent;
        [Toggle("enable")] public CameraDependent cameraDependent;
        [Toggle("enable")] public ExclusionDependent exclusionDependent;
        [Toggle("enable")] public FindDependent findDependent;
        [Toggle("enable")] public SpecialDependent specialDependent;

        public string ChooseText => chooseText;
        public ScreenSceneScriptableObject Scene => scene;
    }

    [Serializable]
    public class ChangeBackGround
    {
        public bool enable;
        public BgEnum bgEnum;
    }
    
    [Serializable]
    public class StatusDependent
    {
        public bool enable = false;
        public StatusesValue[] statusesValues;
    }

    [Serializable]
    public class StatusesValue
    {
        public bool value = true;
        public StatusEnum status = StatusEnum.NONE;
    }
    
    [Serializable]
    public class CameraDependent
    {
        public bool enable;
        public bool isPrepAction;
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
        public StatusesValue[] statusesValues;
    }
    
    [Serializable]
    public class SpecialDependent
    {
        public bool enable;
        public SpecialScriptableObjectBase[] special;
    }
    
    [Serializable]
    public class StatusSetter
    {
        [SerializeField] private bool enable = false;
        [SerializeField] private StatusesValue[] statusesValues;

        public bool Enable => enable;
        public StatusesValue[] StatusesValues => statusesValues;
    }
    
    [Serializable]
    public class CountSetter
    {
        [SerializeField] public bool enable = false;
        [SerializeField] public int count = 1;
        [SerializeField] public CountType type = CountType.NONE;

        public bool Enable => enable;
        public int Count => count;
        public CountType Type => type;
    }
}