using System;
using System.Collections.Generic;
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
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        
        public bool Visible
        {
            set => gameObject.SetActive(value);
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
    }
}