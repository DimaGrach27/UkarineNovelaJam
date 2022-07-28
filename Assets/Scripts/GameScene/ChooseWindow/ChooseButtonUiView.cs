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

        private void Awake()
        {
            Button.onClick.AddListener(ClickButton);
        }

        public void InitButton(NextScene chooseScene, bool isCameraAction)
        {
            textMeshProUGUI.text = chooseScene.ChooseText;

            if (isCameraAction)
            {
                textMeshProUGUI.fontStyle = FontStyles.Underline;
            }
            else
            {
                textMeshProUGUI.fontStyle = FontStyles.Normal;
            }
            
            _chooseScene = chooseScene;
        }

        private void ClickButton() => OnChoose?.Invoke(_chooseScene);
    }
}