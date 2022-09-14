using System;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugStatusToggleElement : MonoBehaviour
    {
        [SerializeField] private Toggle toggleStatus;
        [SerializeField] private Text textName;

        private void Awake()
        {
            toggleStatus.onValueChanged.AddListener(OnChangeValueHandler);
        }

        public string Name
        {
            set => textName.text = value;
        }

        public bool Status
        {
            set => toggleStatus.isOn = value;
        }
        
        public StatusEnum StatusEnum { get; set; }

        private void OnChangeValueHandler(bool value)
        {
            SaveService.SetStatusValue(StatusEnum, value);
        }
    }
}