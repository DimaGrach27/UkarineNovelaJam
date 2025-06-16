using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class ButtonExt : Button
    {
        [SerializeField] private bool m_selectedOnHover = true;

        public void SetSelectedOnHover(bool selectedOnHover)
        {
            m_selectedOnHover = selectedOnHover;
        }
        
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
  
            if (!Cursor.visible || !m_selectedOnHover)
            {
                return;
            }
            
            FocusUIManager.Instance.JumpSelectionToObject(this);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}