using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameModelBlock
{
    [CreateAssetMenu(
        fileName = "Character name", 
        menuName = "Frog Croaked Team/Create 'Character name'", 
        order = 0)]
    
    public class CharacterNameScriptableObject : ScriptableObject
    {
        public CharacterName characterNameType;
        public string characterName;
    }
}