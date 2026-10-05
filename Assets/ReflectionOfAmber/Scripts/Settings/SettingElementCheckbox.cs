using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.Settings
{
    public class SettingElementCheckbox : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        
        public event Action<bool> OnChangeValue;

        private void Awake()
        {
            toggle.onValueChanged.AddListener(ChangeValue);
        }
        
        private void ChangeValue(bool value)
        {
            OnChangeValue?.Invoke(value);
        }

        public void SetValue(bool value)
        {
            toggle.isOn = value;
            ChangeValue(value);
        }
    }
}