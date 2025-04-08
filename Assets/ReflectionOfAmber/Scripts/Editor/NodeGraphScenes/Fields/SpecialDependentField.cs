using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO;
using UnityEngine;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor.Fields
{
    public class SpecialDependentField : PortFieldBase
    {
        private VisualElement m_arrayElement;

        protected override int BlockSize => 32;

        private int m_elementsCount;

        private SpecialDependent m_specialDependent;
        private List<SpecialScriptableObjectBase> m_array;
        
        public SpecialDependentField(SpecialDependent specialDependent, VisualElement container) : base(container)
        {
            m_specialDependent = specialDependent;
            m_array = new(m_specialDependent.special);
            
            m_arrayElement = CreateSpecialArrayField(
                "Specials",
                m_array,
                () =>
                {
                    m_elementsCount = m_array.Count;
                    Debug.Log($"ArrayChanged: {m_elementsCount}");

                    m_specialDependent.special = m_array.ToArray();
                });
        }
        
        protected override void OnEnable()
        {
            m_specialDependent.enable = true;
            
            m_arrayElement.style.marginLeft = ElementMargin;

            LocalContainer.contentContainer.Add(m_arrayElement);
        }

        protected override void OnDisable()
        {
            m_specialDependent.enable = false;

            ReduceSize(ArrayElementSize * m_elementsCount);
            LocalContainer.contentContainer.Remove(m_arrayElement);
        }

        protected override string Title => "Special dependent: ";
    }
}