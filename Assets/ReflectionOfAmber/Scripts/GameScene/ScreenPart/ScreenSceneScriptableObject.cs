using System;
using ReflectionOfAmber.Scripts.Characters;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.Characters;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart
{
    [CreateAssetMenu(
        fileName = "Screen Scene", 
        menuName = "Frog Croaked Team/Create 'Screen Scene'", 
        order = 0)]
    
    public class ScreenSceneScriptableObject : ScriptableObject
    {
        public string sceneKey;
        
        public bool isActiveCamera = false;
        
        public ChangeBackGround changeBackGround;
        public StatusSetter statusSetter;
        public CountSetter countSetter;
        
        public ActionType[] actionsType;

        public ScreenPart[] screenParts;
        public NextScene[] nextScenes;
        
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
        public ActionType[] actionsType;
        public MusicType musicTypeOnStart = MusicType.NONE;
        public StatusSetter statusSetter;
        
        public CharacterScreenPositionEnum screenPosition;
        public CharacterSprite characterImage;
        public CharacterName nameCharacter;
        
        [TextArea(1, 6)] public string textShow;
        public bool endOfText = true;
        public ActionType[] actionsTypeEnd;
        
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
        public ScreenSceneScriptableObject scene = new();
        [TextArea(1, 4)] public string chooseText = String.Empty;

        public StatusDependent statusDependent = new();
        public CameraDependent cameraDependent = new();
        public ExclusionDependent exclusionDependent = new();
        public FindDependent findDependent = new();
        public SpecialDependent specialDependent = new();

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
        public StatusesValue[] statusesValues = Array.Empty<StatusesValue>();
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
        public StatusesValue[] statusesValues = Array.Empty<StatusesValue>();
    }
    
    [Serializable]
    public class SpecialDependent
    {
        public bool enable;
        public SpecialScriptableObjectBase[] special = Array.Empty<SpecialScriptableObjectBase>();
    }
    
    [Serializable]
    public class StatusSetter
    {
        public bool enable = false;
        public StatusesValue[] statusesValues = Array.Empty<StatusesValue>();

        public bool Enable => enable;
        public StatusesValue[] StatusesValues => statusesValues;
    }
    
    [Serializable]
    public class CountSetter
    {
        public bool enable = false;
        public int count = 1;
        public CountType type = CountType.NONE;

        public bool Enable => enable;
        public int Count => count;
        public CountType Type => type;
    }
}