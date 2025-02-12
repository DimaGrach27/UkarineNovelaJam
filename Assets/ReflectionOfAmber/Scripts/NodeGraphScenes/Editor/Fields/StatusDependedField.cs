using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields
{
    public class StatusDependedField : PortFieldBase
    {
        private VisualElement m_arrayElement;

        protected override int BlockSize => 54;

        private int m_elementsCount;

        public StatusDependedField(List<StatusesValue> statusDependents, VisualElement container) : base(container)
        {
            m_arrayElement = CreateStatusesValueArrayField(
                "Statuses",
                statusDependents,
                () =>
                {
                    m_elementsCount = statusDependents.Count;
                    Debug.Log($"ArrayChanged: {m_elementsCount}");
                });
        }

        protected override void OnEnable()
        {
            m_arrayElement.style.marginLeft = ElementMargin;

            LocalContainer.contentContainer.Add(m_arrayElement);
        }

        protected override void OnDisable()
        {
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