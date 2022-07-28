using System;
using System.Collections.Generic;
using GameScene.ScreenPart;
using TMPro;
using UnityEngine;

namespace GameScene.ChooseWindow
{
    public class ChooseWindowUiView : MonoBehaviour
    {
        public event Action<NextScene> OnChoose;

        [SerializeField] private TextMeshProUGUI chooseText;
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
                    ChooseButtonUiView chooseButtonUiView = Instantiate(buttonPrefab, container);
                    chooseButtonUiView.OnChoose += OnButtonChooseClick;
                    
                    _chooseButtonUiViews.Add(chooseButtonUiView);
                }

                _chooseButtonUiViews[i].InitButton(scenes[i], isCameraAction);
                _chooseButtonUiViews[i].gameObject.SetActive(true);
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
    }
}