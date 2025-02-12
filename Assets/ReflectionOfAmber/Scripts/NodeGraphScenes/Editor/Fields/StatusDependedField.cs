using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields
{
    public class StatusDependedField : PortFieldBase
    {
        private VisualElement m_arrayElement;
        private List<StatusesValue> m_statuses;

        private StatusDependent m_statusDependent;
        
        protected override int BlockSize => 54;

        private int m_elementsCount;

        public StatusDependedField(StatusDependent statusDependent, VisualElement container) : base(container)
        {
            m_statusDependent = statusDependent;
            m_statuses = new List<StatusesValue>(m_statusDependent.statusesValues);

            m_arrayElement = CreateStatusesValueArrayField(
                "Statuses",
                m_statuses,
                () =>
                {
                    m_elementsCount = m_statuses.Count;
                    Debug.Log($"ArrayChanged: {m_elementsCount}");

                    m_statusDependent.statusesValues = m_statuses.ToArray();
                });
        }

        protected override void OnEnable()
        {
            m_statusDependent.enable = true;
            
            m_arrayElement.style.marginLeft = ElementMargin;

            LocalContainer.contentContainer.Add(m_arrayElement);
        }

        protected override void OnDisable()
        {
            m_statusDependent.enable = false;

            ReduceSize(ArrayElementSize * m_elementsCount);
            LocalContainer.contentContainer.Remove(m_arrayElement);

            // float height = LocalContainer.resolvedStyle.height;
            // height -= ArrayElementSize * m_elementsCount;
            //
            // LocalContainer.style.height = height;
        }

        protected override string Title => "Status depended: ";
    }
}