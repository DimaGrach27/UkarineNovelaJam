using ReflectionOfAmber.Scripts;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Count SO", 
        menuName = "Frog Croaked Team/Special/Create 'Ability SO'", 
        order = 0)]
    
    public class AbilitySpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private NextScene beliefScene;
        [SerializeField] private NextScene escapeScene;
        [SerializeField] private NextScene wastedScene;
        
        [SerializeField] private CountType escape;
        [SerializeField] private CountType belief;

        private NextScene _returnScene;
        
        public override bool Check()
        {
            int escapeCount = SaveService.GetIntValue(escape);
            int beliefCount = SaveService.GetIntValue(belief);

            if (escapeCount > 1 || beliefCount > 0)
            {
                if (beliefCount == 2 || beliefCount >= escapeCount)
                {
                    _returnScene = beliefScene;
                    return true;
                }
            
                if (escapeCount == 3 || escapeCount >= beliefCount)
                {
                    _returnScene = escapeScene;
                    return true;
                }
            }
            
            _returnScene = wastedScene;
            return true;
        }

        public override NextScene NextScene => _returnScene;
    }
}