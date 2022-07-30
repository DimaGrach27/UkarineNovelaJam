using UnityEngine;

namespace Characters
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