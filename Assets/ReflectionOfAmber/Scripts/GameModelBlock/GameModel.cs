using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public static class GameModel
    {
        public static bool IsGamePlaying = false;
        public static float TYPING_SPEED = 0.05f;
        
        private static readonly Dictionary<string, ScreenSceneScriptableObject> ScreenScenesMap = new();
        private static readonly Dictionary<CharacterName, CharacterNameScriptableObject> CharacterNameMap = new();
        private static readonly Dictionary<BgEnum, BgScriptableObject> _bgMap = new();

        public static TranslatorLanguages CurrentLanguage = TranslatorLanguages.ENG;

        public static void Init()
        {
            ScreenSceneScriptableObject[] list =
                Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            CharacterNameScriptableObject[] listNames =
                Resources.LoadAll<CharacterNameScriptableObject>("Configs/CharacterNames");
            
            BgScriptableObject[] bgScriptableObjects =
                Resources.LoadAll<BgScriptableObject>("Configs/BackGrounds");
            
            foreach (var screenScene in list)
            {
                ScreenScenesMap.Add(screenScene.SceneKey, screenScene);
            }
            
            foreach (var characterName in listNames)
            {
                CharacterNameMap.Add(characterName.characterNameType, characterName);
            }
            
            foreach (var bgScriptable in bgScriptableObjects)
            {
                _bgMap.Add(bgScriptable.Bg, bgScriptable);
            }
        }

        public static ScreenSceneScriptableObject GetScene(string key)
        {
            if (ScreenScenesMap.ContainsKey(key))
                return ScreenScenesMap[key];

            return null;
        }

        public static string GetName(CharacterName characterName)
        {
            string result = "";

            if (!CharacterNameMap.ContainsKey(characterName)) return result;

            result = CharacterNameMap[characterName].characterName;
            return result;
        }
        
        public static Sprite GetBg(BgEnum bgEnum)
        {
            Sprite sprite = null;

            if (_bgMap.ContainsKey(bgEnum))
            {
                sprite = _bgMap[bgEnum].Image;
            }

            return sprite;
        }
        
        public static AnimationScreen GetAnimationScreen(BgEnum bgEnum)
        {
            AnimationScreen animationScreen = null;

            if (_bgMap.ContainsKey(bgEnum))
            {
                animationScreen = _bgMap[bgEnum].AnimationScreen;
            }

            return animationScreen;
        }
    }
}