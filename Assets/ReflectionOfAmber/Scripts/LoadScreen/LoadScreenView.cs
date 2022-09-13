using System;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.LoadScreen
{
    public class LoadScreenView : MonoBehaviour
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private NoteWindowSaveScreenPart[] buttons;
        
        public event Action<int> OnCLickButton;
        public event Action OnCloseClick;

        public int Count => buttons.Length;

        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        private Tween _fade;
        
        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Index = i;
                buttons[i].OnCLickButton += OnCLickButtonHandler;
            }
            
            closeButton.onClick.AddListener(OnCLickCloseButtonHandler);
        }

        private void OnCLickButtonHandler(int index) => OnCLickButton?.Invoke(index);
        private void OnCLickCloseButtonHandler()
        {
            FadeOutWindow();
            OnCloseClick?.Invoke();
        }

        public void UpdateElement(int index, Sprite sprite, bool isHaveSave, string description)
        {
            buttons[index].Sprite = sprite;
            buttons[index].IsHaveSave = isHaveSave;
            buttons[index].Description = description;
        }

        public void Open()
        {
            FadeInWindow();
        }
        
        
        private void FadeInWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(1.0f, duration);
        }
        
        private void FadeOutWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(0.0f, duration);
        }
    }
}