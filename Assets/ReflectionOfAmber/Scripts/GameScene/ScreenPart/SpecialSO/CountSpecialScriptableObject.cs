using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Count SO", 
        menuName = "Frog Croaked Team/Special/Create 'Count SO'", 
        order = 0)]
    
    public class CountSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusEnum status;
        [SerializeField] private bool valueStatus = true;
        
        [SerializeField] private CountType countType;
        [SerializeField] private int valueCount;
        
        public override bool Check()
        {
            bool isCount = SaveService.GetIntValue(countType) >= valueCount;

            isCount &= SaveService.GetStatusValue(status) == valueStatus;
            
            return isCount;
        }
    }
}