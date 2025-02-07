using ReflectionOfAmber.Scripts.GameModelBlock;
using UnityEngine.UIElements;

namespace ReflectionOfAmber.Scripts.NodeGraphScenes.Editor
{
    public class StatusSetterUIElement : VisualElement
    {
        private Toggle m_valueToggle;
        private EnumField m_valueEnum;
        
        public StatusSetterUIElement(SceneNode node)
        {
            var label = new Label("Status setter:");
            var enableToggle = new Toggle("Enable");
            
            enableToggle.SetValueWithoutNotify(false);
            enableToggle.RegisterValueChangedCallback(evt =>
            {
                if (evt.newValue)
                {
                    m_valueToggle = new Toggle("Value");
                    m_valueEnum = new EnumField("Status", StatusEnum.NONE);
                    
                    Add(m_valueToggle);
                    Add(m_valueEnum);
                }
                else
                {
                    Remove(m_valueToggle);
                    Remove(m_valueEnum);
                }
                
                enableToggle.value = evt.newValue;
                // node.StatusSetter.enable = enableToggle.value;
            });

            Add(label);
            Add(enableToggle);
        }
    }
}