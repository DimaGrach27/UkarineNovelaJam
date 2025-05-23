using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow
{
    public class ChooseWindowUiView : MonoBehaviour
    {
        public event Action<NextScene> OnChoose;

        [SerializeField] private TextMeshProUGUI chooseText;
        [SerializeField] private ScrollRect container;
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
        
        public void InitButtons(NextScene[] scenes, bool isCameraAction)
        {
            foreach (var choose in _chooseButtonUiViews)
            {
                choose.gameObject.SetActive(false);
            }
            
            for (int i = 0; i < scenes.Length; i++)
            {
                if (i > _chooseButtonUiViews.Count - 1)
                {
                    ChooseButtonUiView chooseButtonUiView = Instantiate(buttonPrefab, container.content);
                    chooseButtonUiView.OnChoose += OnButtonChooseClick;
                    
                    _chooseButtonUiViews.Add(chooseButtonUiView);
                }

                _chooseButtonUiViews[i].gameObject.SetActive(true);
                _chooseButtonUiViews[i].InitButton(scenes[i], isCameraAction);
            }
        }

        public void SetChooseText(string textChoose)
        {
            chooseText.SetText(textChoose);
        }

        private void OnButtonChooseClick(NextScene choose)
        {
            OnChoose?.Invoke(choose);
        }
        
        void Update()
        {
            MoveScrollbar();
        }

        private void MoveScrollbar()
        {
            container.verticalScrollbar.ScrollByMouseWheel();
        }
    }
}