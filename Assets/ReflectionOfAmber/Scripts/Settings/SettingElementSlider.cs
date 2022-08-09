using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.Settings
{
    public class SettingElementSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TextMeshProUGUI textMeshPro;

        public event Action<float> OnChangeValue;

        private void Awake()
        {
            slider.onValueChanged.AddListener(ChangeValue);
        }

        private void ChangeValue(float value)
        {
            textMeshPro.text = value.ToString("00");
            OnChangeValue?.Invoke(value);
        }

        public void SetValue(float value)
        {
            slider.value = value;
            ChangeValue(value);
        }
    }
}