using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.UI;
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

        private Coroutine m_PostOpenRoutine;
        
        public bool Visible
        {
            set => gameObject.SetActive(value);
        }
        
        public void InitButtons(NextScene[] scenes, bool isCameraAction)
        {
            FocusUIManager.Instance.ResetSelectionObject();
            
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

                    if (i > 0)
                    {
                        ChooseButtonUiView prevBtnUiView = _chooseButtonUiViews[i - 1];
                        ChooseButtonUiView currBtnUiView = _chooseButtonUiViews[i];
                        
                        Navigation prevButtonNavigation = prevBtnUiView.Button.navigation;
                        Navigation currButtonNavigation = currBtnUiView.Button.navigation;

                        prevButtonNavigation.selectOnDown = currBtnUiView.Button;
                        currButtonNavigation.selectOnUp = prevBtnUiView.Button;
                        
                        prevBtnUiView.Button.navigation = prevButtonNavigation;
                        currBtnUiView.Button.navigation = currButtonNavigation;
                    }
                }

                _chooseButtonUiViews[i].gameObject.SetActive(true);
                _chooseButtonUiViews[i].InitButton(scenes[i], isCameraAction);
            }

            if (m_PostOpenRoutine != null)
            {
                StopCoroutine(m_PostOpenRoutine);
            }

            m_PostOpenRoutine = StartCoroutine(PostOpenRoutine());
        }

        private IEnumerator PostOpenRoutine()
        {
            yield return null;
            FocusUIManager.Instance.JumpSelectionToObject(_chooseButtonUiViews[0].Button);
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