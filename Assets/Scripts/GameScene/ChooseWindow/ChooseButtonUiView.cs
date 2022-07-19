using System;
using GameScene.ScreenPart;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.ChooseWindow
{
    public class ChooseButtonUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        
        public event Action<NextScene> OnChoose; 
        
        private Button _button;
        private Button Button => _button ??= GetComponent<Button>();

        private NextScene _chooseScene;

        public void InitButton(NextScene chooseScene)
        {
            textMeshProUGUI.text = chooseScene.ChooseText;
            _chooseScene = chooseScene;
            Button.onClick.AddListener(ClickButton);
        }

        private void ClickButton() => OnChoose?.Invoke(_chooseScene);
    }
}