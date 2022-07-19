using System;
using System.Collections.Generic;
using DG.Tweening;
using GameScene.ScreenPart;
using UnityEngine;

namespace GameScene.ChooseWindow
{
    public class ChooseWindowUiView : MonoBehaviour
    {
        public event Action<NextScene> OnChoose;
        
        [SerializeField] private Transform container;
        [SerializeField] private ChooseButtonUiView buttonPrefab;

        private readonly List<ChooseButtonUiView> _chooseButtonUiViews = new();
        
        private CanvasGroup _canvasGroup;
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        private bool _isVisible;

        public bool Visible
        {
            get => _isVisible;
            
            set
            {
                if (Visible && !value)
                {
                    DissolveOut();
                }
                
                if (!Visible && value)
                {
                    DissolveIn();
                }
                
                _isVisible = value;
                gameObject.SetActive(_isVisible);
            }
        }
        
        public void InitButtons(NextScene[] scenes)
        {
            foreach (var choose in _chooseButtonUiViews)
            {
                choose.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < scenes.Length; i++)
            {
                if (i > _chooseButtonUiViews.Count - 1)
                {
                    ChooseButtonUiView chooseButtonUiView = Instantiate(buttonPrefab, container);
                    chooseButtonUiView.OnChoose += OnButtonChooseClick;
                    
                    _chooseButtonUiViews.Add(chooseButtonUiView);
                }

                _chooseButtonUiViews[i].InitButton(scenes[i]);
                _chooseButtonUiViews[i].gameObject.SetActive(true);
            }
        }

        private void OnButtonChooseClick(NextScene choose)
        {
            OnChoose?.Invoke(choose);
        }
        
        private void DissolveIn(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            CanvasGroup.alpha = 0.0f;
            CanvasGroup.DOFade(1.0f, duration);
        }
        
        private void DissolveOut(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            CanvasGroup.alpha = 1.0f;
            CanvasGroup.DOFade(0.0f, duration);
        }
    }
}