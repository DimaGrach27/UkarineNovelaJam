using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.UI
{
    public class SliderExt : Slider
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