using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "DubleOr SO", 
        menuName = "Frog Croaked Team/Special/Create 'DubleOr SO'", 
        order = 0)]
    
    public class DubleOrSpecialSO : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusDependent statusEnum;
        
        [SerializeField] private NextScene blockTrue;
        [SerializeField] private NextScene blockFalse;

        private NextScene _nextScene;
        public override bool Check()
        {
            bool result = true;

            foreach (var statusesValue in statusEnum.statusesValues)
            {
                result &= statusesValue.value == GameModel.GetStatus(statusesValue.status);
            }
            
            _nextScene = result ? blockTrue : blockFalse;
            
            return true;
        }
        
        public override NextScene NextScene => _nextScene;
    }
}