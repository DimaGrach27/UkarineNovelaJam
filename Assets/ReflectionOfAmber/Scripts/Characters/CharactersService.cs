using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Characters
{
    public static class CharactersService
    {
        private static  readonly Dictionary<CharacterSprite, CharacterSpriteScriptableObject> CharacterSpriteMap = new();
        
        static CharactersService()
        {
            foreach (var characterSprite in Resources.LoadAll<CharacterSpriteScriptableObject>("Configs/Characters"))
            {
                CharacterSpriteMap.Add(characterSprite.typeSprite, characterSprite);
            }
        }

        public static Sprite GetSprite(CharacterSprite characterSprite)
        {
            if (!CharacterSpriteMap.ContainsKey(characterSprite)) return null;

            return CharacterSpriteMap[characterSprite].sprite;
        }
    }
}