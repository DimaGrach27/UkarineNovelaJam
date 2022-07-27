using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScene
{
    public class ClickHelper : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}