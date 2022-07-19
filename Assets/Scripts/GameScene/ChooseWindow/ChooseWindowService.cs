using System;
using GameScene.ScreenPart;
using UnityEngine;

namespace GameScene.ChooseWindow
{
    public class ChooseWindowService
    {
        private readonly ChooseWindowUiView _chooseWindowUiView;

        public event Action<NextScene> OnChoose; 

        public ChooseWindowService(Transform transform)
        {
            _chooseWindowUiView = transform.GetComponentInChildren<ChooseWindowUiView>();
            _chooseWindowUiView.OnChoose += OnChooseClick;
        }

        public void SetChooses(NextScene[] nextScenes)
        {
            _chooseWindowUiView.Visible = true;
            _chooseWindowUiView.InitButtons(nextScenes);
        }

        public void ChangeVisible(bool isVisible) => _chooseWindowUiView.Visible = isVisible;
        private void OnChooseClick(NextScene chooseScene)
        {
            OnChoose?.Invoke(chooseScene);
            _chooseWindowUiView.Visible = false;
        }
    }
}