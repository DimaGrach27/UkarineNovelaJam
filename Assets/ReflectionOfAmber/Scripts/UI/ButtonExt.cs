using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class ButtonExt : Button
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
  
            if (!Cursor.visible)
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