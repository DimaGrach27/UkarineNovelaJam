using System;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Input
{
    [CreateAssetMenu(fileName = "UiInputHintsConfig", menuName = "Frog Croaked Team/Create new 'UiInputHintsConfig'", order = 0)]
    public class UiInputHintsConfig : ScriptableObject
    {
        [SerializeField] 
        private UIHintData[] UIHintData;

        public UIHintData[] GetUiHintData()
        {
            return UIHintData;
        }
    }

    [Serializable]
    public struct UIHintData
    {
        public DeviceType DeviceType;
        public KeybindingData[] KeybindingData;
    }

    [Serializable]
    public struct KeybindingData
    {
        public KeyCodePath ActionID;
        public Sprite Icon;
    }
}