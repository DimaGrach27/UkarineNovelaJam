﻿using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields
{
    public class CameraDependedField : PortFieldBase
    {
        private Toggle m_prepToggle;
        private Toggle m_visibleToggle;
        private Toggle m_outVisibleToggle;

        private CameraDependent m_cameraDependent;
        
        public CameraDependedField(CameraDependent cameraDependent, VisualElement container) : base(container)
        {
            m_cameraDependent = cameraDependent;
            
            m_prepToggle = CreateToggleField(
                "IsPrepAction",
                value => cameraDependent.isPrepAction = value,
                m_cameraDependent.isPrepAction);
            
            m_visibleToggle = CreateToggleField(
                "VisibleOnPhoto",
                value => cameraDependent.visibleOnPhoto = value,
                m_cameraDependent.visibleOnPhoto);
            
            m_outVisibleToggle = CreateToggleField(
                "VisibleOutPhoto",
                value => cameraDependent.visibleOutPhoto = value,
                m_cameraDependent.visibleOutPhoto);

        }

        protected override void OnEnable()
        {
            // LocalContainer.style.borderBottomWidth = 0;
            m_cameraDependent.enable = true;
            
            m_prepToggle.style.marginLeft = ElementMargin;
            m_visibleToggle.style.marginLeft = ElementMargin;
            m_outVisibleToggle.style.marginLeft = ElementMargin;

            LocalContainer.contentContainer.Add(m_prepToggle);
            LocalContainer.contentContainer.Add(m_visibleToggle);
            LocalContainer.contentContainer.Add(m_outVisibleToggle);
            
            // LocalContainer.style.borderBottomWidth = BottomSpace;
        }

        protected override void OnDisable()
        {
            m_cameraDependent.enable = false;
            
            LocalContainer.contentContainer.Remove(m_prepToggle);
            LocalContainer.contentContainer.Remove(m_visibleToggle);
            LocalContainer.contentContainer.Remove(m_outVisibleToggle);
        }

        protected override string Title => "Camera dependent: ";
    }
}