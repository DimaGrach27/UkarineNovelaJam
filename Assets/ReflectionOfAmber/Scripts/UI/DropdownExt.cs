using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReflectionOfAmber.Scripts.UI
{
    public class DropdownExt : TMP_Dropdown
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
    }
}