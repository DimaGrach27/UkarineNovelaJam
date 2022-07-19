using UnityEngine;

namespace GameScene.ScreenText
{
    public class ScreenTextService
    {
        private readonly ScreenTextUiView _screenTextUiView;

        public ScreenTextService(Transform uiTransform)
        {
            _screenTextUiView = uiTransform.GetComponentInChildren<ScreenTextUiView>();
        }

        public void ShowText()
        {
            _screenTextUiView.Visible = true;
        }
        
        public void SetText(string name, string text)
        {
            _screenTextUiView.Name = name;
            _screenTextUiView.Text = text;
        }
        
        public void HideText()
        {
            _screenTextUiView.Visible = false;
        }
    }
}