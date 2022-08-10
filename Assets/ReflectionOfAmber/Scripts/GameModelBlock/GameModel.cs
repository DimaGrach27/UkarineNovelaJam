using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public static class GameModel
    {
        public static bool IsGamePlaying = false;
        public static float TYPING_SPEED = 0.05f;
        
        private static readonly Dictionary<string, ScreenSceneScriptableObject> ScreenScenesMap = new();
        private static readonly Dictionary<CharacterName, CharacterNameScriptableObject> CharacterNameMap = new();

        public static void Init()
        {
            ScreenSceneScriptableObject[] list =
                Resources.LoadAll<ScreenSceneScriptableObject>("Configs/Screens");

            CharacterNameScriptableObject[] listNames =
                Resources.LoadAll<CharacterNameScriptableObject>("Configs/CharacterNames");
            
            foreach (var screenScene in list)
            {
                ScreenScenesMap.Add(screenScene.SceneKey, screenScene);
            }
            
            foreach (var characterName in listNames)
            {
                CharacterNameMap.Add(characterName.characterNameType, characterName);
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
    }
}