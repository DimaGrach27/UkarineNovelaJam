﻿using System;
using System.Collections;
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

        private Coroutine _coroutine;
        
        private void Awake()
        {
            Button.onClick.AddListener(ClickButton);
        }

        public void InitButton(NextScene chooseScene, bool isCameraAction)
        {
            textMeshProUGUI.text = chooseScene.ChooseText;
            _chooseScene = chooseScene;

            ColorBlock colorBlock = Button.colors;
            colorBlock.normalColor = Color.black;
            Button.colors = colorBlock;
            
            if(!isCameraAction) return;
            
            if(_coroutine != null) StopCoroutine(_coroutine);
            
            if (_chooseScene.cameraDependent.isPrepAction)
            {
                _coroutine = StartCoroutine(ChooseBlinkRoutine());
            }
        }

        private void ClickButton() => OnChoose?.Invoke(_chooseScene);

        private IEnumerator ChooseBlinkRoutine()
        {
            while (true)
            {
                ColorBlock colorBlock = Button.colors;
                Color colorButton;
                
                for (float i = 0; i <= 1.0f; i += Time.deltaTime * 1.5f)
                {
                    colorButton = Color.Lerp(Color.black, Color.green, i);
                    colorBlock.normalColor = colorButton;
                    Button.colors = colorBlock;
                    yield return null;
                }
                
                for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime * 1.5f)
                {
                    colorButton = Color.Lerp(Color.black, Color.green, i);
                    colorBlock.normalColor = colorButton;
                    Button.colors = colorBlock;
                    yield return null;
                }
            }
        }
    }
}