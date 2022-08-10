using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    public static class GameModel
    {
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
            switch (characterName)
            {
                case CharacterName.NONE:
                    result = GlobalConstant.NONE_NAME;
                    break;
                case CharacterName.VILSHANKA:
                    result = GlobalConstant.VILSHANKA_NAME;
                    break;
                case CharacterName.VILSHANKA_FUTURE:
                    result = GlobalConstant.VILSHANKA_FUTURE_NAME;
                    break;
                case CharacterName.ILONA:
                    result = GlobalConstant.ILONA_NAME;
                    break;
                case CharacterName.ZAHARES:
                    result = GlobalConstant.ZAHARES_NAME;
                    break;
                case CharacterName.OLEKSIY:
                    result = GlobalConstant.OLEKSIY_NAME;
                    break;
                case CharacterName.OREST:
                    result = GlobalConstant.OREST_NAME;
                    break;
                case CharacterName.UNKNOW:
                    result = GlobalConstant.UNKNOW_NAME;
                    break;
                case CharacterName.ILONA_SUR:
                    result = GlobalConstant.ILONA_SURNAME;
                    break;
                case CharacterName.ZAHARES_SUR:
                    result = GlobalConstant.ZAHARES_SUR_NAME;
                    break;
                case CharacterName.OLEKSIY_SUR:
                    result = GlobalConstant.OLEKSIY_SUR_NAME;
                    break;
            }

            return result;
        }
    }
}