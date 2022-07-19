using UnityEngine;

namespace GameScene.ScreenText
{
    public class ScreenTextService
    {
        private readonly ScreenServiceUiView _screenServiceUiView;

        public ScreenTextService(Transform uiTransform)
        {
            _screenServiceUiView = uiTransform.GetComponentInChildren<ScreenServiceUiView>();
        }

        public void ShowText()
        {
            _screenServiceUiView.Visible = true;
        }
        
        public void SetText(string name, string text)
        {
            _screenServiceUiView.Name = name;
            _screenServiceUiView.Text = text;
        }
        
        public void HideText()
        {
            _screenServiceUiView.Visible = false;
        }
    }
}