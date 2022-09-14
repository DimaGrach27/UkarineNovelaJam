using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.DebugHelper
{
    public class DebugStatusHelperView : MonoBehaviour
    {
        [SerializeField] private Toggle openClose;
        [SerializeField] private DebugStatusToggleElement debugStatusToggleElement;
        [SerializeField] private Transform container;

        private CanvasGroup _canvasGroup;
        private readonly Dictionary<StatusEnum, DebugStatusToggleElement> _statusMap = new();

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            openClose.onValueChanged.AddListener(OpenClose);

            openClose.isOn = false;
            

            int countOfStatus = Enum.GetNames(typeof(StatusEnum)).Length;

            for (int i = 0; i < countOfStatus; i++)
            {
                DebugStatusToggleElement statusToggleElement = Instantiate(debugStatusToggleElement, container);
                StatusEnum statusEnum = (StatusEnum)i;
                
                statusToggleElement.Name = statusEnum.ToString();
                // statusToggleElement.Status = SaveService.GetStatusValue(statusEnum);
                statusToggleElement.StatusEnum = statusEnum;
                
                _statusMap.Add(statusEnum, statusToggleElement);
            }
        }

        private void OpenClose(bool isOpen)
        {
            _canvasGroup.alpha = isOpen ? 1.0f : 0.0f;
            _canvasGroup.interactable = isOpen;
            _canvasGroup.blocksRaycasts = isOpen;
            
            if(!isOpen) return;

            foreach (var keyValue in _statusMap)
            {
                keyValue.Value.Status = SaveService.GetStatusValue(keyValue.Key);
            }
        }
    }
}