using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    public abstract class SpecialScriptableObjectBase : ScriptableObject
    {
        [SerializeField] private NextScene nextScene;
        
        protected ScreenPartsServiceFacade ServiceFacade;
        public ScreenPartsServiceFacade SetService
        {
            set => ServiceFacade = value;
        } 
        
        public abstract bool Check();
        public virtual NextScene NextScene => nextScene;
    }
}