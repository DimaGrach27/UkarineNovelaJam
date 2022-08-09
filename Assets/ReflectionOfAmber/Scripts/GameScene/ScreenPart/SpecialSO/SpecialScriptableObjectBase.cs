using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    public abstract class SpecialScriptableObjectBase : ScriptableObject
    {
        [SerializeField] private NextScene nextScene;
        
        public abstract bool Check();
        public virtual NextScene NextScene => nextScene;
    }
}