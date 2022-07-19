using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameScene.Services
{
    public class UiClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClick;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke();
        }
    }
}