using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public class GameModel : IInit
    {
        private readonly GameResourcesService m_gameResourcesService;
        
        public static bool IsGamePlaying = false;
        public static float TYPING_SPEED = 0.05f;
        
        private static Dictionary<string, ScreenSceneScriptableObject> m_ScreenScenesMap = new();
        private static Dictionary<CharacterName, CharacterNameScriptableObject> m_CharacterNameMap = new();
        private static Dictionary<BgEnum, BgScriptableObject> m_BgMap = new();

        public static TranslatorLanguages CurrentLanguage = TranslatorLanguages.UKR;

        public event Action OnReady;

        [Inject]
        public GameModel(GameResourcesService gameResourcesService)
        {
            m_gameResourcesService = gameResourcesService;
        }
        
        public void Init()
        {
            // ScreenSceneScriptableObject[] list = Resources.LoadAll<ScreenSceneScriptableObject>("Graphs");
            // // ScreenSceneScriptableObject[] list = Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            // CharacterNameScriptableObject[] listNames = Resources.LoadAll<CharacterNameScriptableObject>("Configs/CharacterNames");
            //
            // BgScriptableObject[] bgScriptableObjects = Resources.LoadAll<BgScriptableObject>("Configs/BackGrounds");
            //
            // foreach (var screenScene in list)
            // {
            //     m_ScreenScenesMap.Add(screenScene.SceneKey, screenScene);
            // }
            //
            // foreach (var characterName in listNames)
            // {
            //     m_CharacterNameMap.Add(characterName.characterNameType, characterName);
            // }
            //
            // foreach (var bgScriptable in bgScriptableObjects)
            // {
            //     m_BgMap.Add(bgScriptable.Bg, bgScriptable);
            // }

            m_ScreenScenesMap = m_gameResourcesService.ScreenScenesMap;
            m_CharacterNameMap = m_gameResourcesService.CharacterNameMap;
            m_BgMap = m_gameResourcesService.BgMap;

            CurrentLanguage = SaveService.LanguageStatus;
            
            OnReady?.Invoke();
        }

        public static ScreenSceneScriptableObject GetScene(string key)
        {
            if (m_ScreenScenesMap.ContainsKey(key))
                return m_ScreenScenesMap[key];

            return null;
        }

        public static string GetName(CharacterName characterName)
        {
            string result = "";

            if (!m_CharacterNameMap.ContainsKey(characterName)) return result;

            result = m_CharacterNameMap[characterName].characterName;
            return result;
        }
        
        public static Sprite GetBg(BgEnum bgEnum)
        {
            Sprite sprite = null;

            if (m_BgMap.ContainsKey(bgEnum))
            {
                sprite = m_BgMap[bgEnum].Image;
            }

            return sprite;
        }
        
        public static AnimationScreen GetAnimationScreen(BgEnum bgEnum)
        {
            AnimationScreen animationScreen = null;

            if (m_BgMap.ContainsKey(bgEnum))
            {
                animationScreen = m_BgMap[bgEnum].AnimationScreen;
            }

            return animationScreen;
        }
    }
}