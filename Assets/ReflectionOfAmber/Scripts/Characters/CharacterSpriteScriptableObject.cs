using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GameScene.BgScreen;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Characters
{
    [CreateAssetMenu(
        fileName = "Character Sprite", 
        menuName = "Frog Croaked Team/Create 'Character Sprite'", 
        order = 0)]

    public class CharacterSpriteScriptableObject : ScriptableObject
    {
        public CharacterSprite typeSprite;
        public Sprite sprite;
    }
}