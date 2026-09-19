using System;
using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow
{
    public class ChooseButtonUiView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        [SerializeField, Min(0.01f)] private float glowPulseDuration = 1.333333f;
        
        public event Action<NextScene> OnChoose; 
        
        private ButtonExt _button;
        private ButtonExt Button => _button ??= GetComponent<ButtonExt>();

        private NextScene _chooseScene;

        private Coroutine _coroutine;
        private Material _textMaterial;
        private const float MaxGlowOuter = 0.1f;
        private bool _hasGlow;
        
        private void Awake()
        {
            Button.onClick.AddListener(ClickButton);
            // TMP creates a private instance so other labels keep their material settings.
            _textMaterial = textMeshProUGUI.fontMaterial;
            _hasGlow = _textMaterial.HasProperty(ShaderUtilities.ID_GlowOuter);
            StopGlowPulse();
        }

        public void InitButton(NextScene chooseScene, bool isCameraAction)
        {
            StopGlowPulse();
            string showText = TranslatorService.GetText(chooseScene.Scene.SceneKey);
            
            textMeshProUGUI.text = showText;
            // textMeshProUGUI.text = chooseScene.ChooseText;
            _chooseScene = chooseScene;

            ColorBlock colorBlock = Button.colors;
            colorBlock.normalColor = Color.black;
            Button.colors = colorBlock;
            
            if(!isCameraAction) return;
            
            if (_hasGlow && (_chooseScene.cameraDependent.isPrepAction || _chooseScene.cameraDependent.visibleOnPhoto))
            {
                _textMaterial.EnableKeyword(ShaderUtilities.Keyword_Glow);
                _coroutine = StartCoroutine(ChooseGlowPulseRoutine());
            }
        }

        private void ClickButton()
        {
            Debug.Log($"Choose scene choosed: {_chooseScene.Scene.SceneKey}");
            OnChoose?.Invoke(_chooseScene);
        }

        private void OnDisable()
        {
            StopGlowPulse();
        }

        private void OnDestroy()
        {
            if (_textMaterial != null)
                Destroy(_textMaterial);
        }

        private void StopGlowPulse()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }

            if (_textMaterial != null)
            {
                _textMaterial.DisableKeyword(ShaderUtilities.Keyword_Glow);
                SetGlowPulse(0f);
            }
        }

        private void SetGlowPulse(float intensity)
        {
            if (_hasGlow)
            {
                _textMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, MaxGlowOuter * intensity);
                textMeshProUGUI.UpdateMeshPadding();
                // UI Mask caches a separate material and does not refresh its shader properties.
                Material renderingMaterial = textMeshProUGUI.materialForRendering;
                if (renderingMaterial != null && renderingMaterial != _textMaterial)
                {
                    renderingMaterial.SetFloat(ShaderUtilities.ID_GlowOuter, MaxGlowOuter * intensity);
                    renderingMaterial.SetFloat(ShaderUtilities.ID_ScaleRatio_B,
                        _textMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_B));
                    if (_textMaterial.IsKeywordEnabled(ShaderUtilities.Keyword_Glow))
                        renderingMaterial.EnableKeyword(ShaderUtilities.Keyword_Glow);
                    else
                        renderingMaterial.DisableKeyword(ShaderUtilities.Keyword_Glow);
                }
                textMeshProUGUI.SetVerticesDirty();
                textMeshProUGUI.SetMaterialDirty();
            }
        }

        private IEnumerator ChooseGlowPulseRoutine()
        {
            float phase = 0f;
            while (true)
            {
                SetGlowPulse((1f - Mathf.Cos(phase * Mathf.PI * 2f)) * 0.5f);
                yield return null;
                phase = Mathf.Repeat(phase + Time.unscaledDeltaTime / Mathf.Max(0.01f, glowPulseDuration), 1f);
            }
        }
    }
}
