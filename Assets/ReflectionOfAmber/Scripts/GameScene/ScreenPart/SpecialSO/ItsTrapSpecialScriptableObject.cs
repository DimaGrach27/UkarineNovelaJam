using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Its Trap SO", 
        menuName = "Frog Croaked Team/Special/Create 'Its Trap SO'", 
        order = 0)]
    
    public class ItsTrapSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusDependent statusDependent;
        public override bool Check()
        {
            if (!statusDependent.enable) return false;

            bool result = true;

            foreach (var statusesValue in statusDependent.statusesValues)
            {
                result &= GameModel.GetStatus(statusesValue.status) == statusesValue.value;
            }
            
            return result;
        }
    }
}