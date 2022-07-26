using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    public abstract class SpecialScriptableObjectBase : ScriptableObject
    {
        [SerializeField] private NextScene nextScene;
        
        public abstract bool Check();
        public virtual NextScene NextScene => nextScene;
    }
}