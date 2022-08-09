using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ReflectionOfAmber.Scripts.GameScene
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