using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    public abstract class SpecialScriptableObjectBase : ScriptableObject
    {
        public abstract bool Check();
        public abstract NextScene NextScene();
    }
}