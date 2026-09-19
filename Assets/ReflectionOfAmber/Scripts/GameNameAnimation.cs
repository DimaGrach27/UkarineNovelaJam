using System;
using System.Collections;
using ReflectionOfAmber.Scripts.FadeScreen;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts
{
    public class GameNameAnimation : MonoBehaviour
    {
        public static GameNameAnimation Inst { get; private set; }

        [SerializeField] private Material fontMaterial;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [Tooltip("Optional sprite target. When assigned, animates this Image instead of the text.")]
        [SerializeField] private Image imageEng;
        [SerializeField] private Image imageUkr;
        [SerializeField] private Material imageMaterial;

        private CanvasGroup _canvasGroup;
        private FadeService _fadeService;
        private Material _runtimeFontMaterial;
        private Material _originalFontMaterial;
        private Graphic _target;
        private Image _activeImage;
        private Material _originalEnglishMaterial;
        private Material _originalUkrainianMaterial;
        private TranslatorService _translatorService;
        private uint? _languageSubscription;

        private static readonly int Bevel = Shader.PropertyToID("_Bevel");
        private static readonly int LightAngle = Shader.PropertyToID("_LightAngle");

        [Inject]
        public void Construct(FadeService fadeService, TranslatorService translatorService)
        {
            _fadeService = fadeService;
            _translatorService = translatorService;
            _languageSubscription = translatorService.Subscribe(this, RefreshLanguage);
        }
        
        private void Awake()
        {
            if (Inst != null) return;

            Inst = this;
            _canvasGroup = GetComponent<CanvasGroup>();
            
            _activeImage = GetLanguageImage();
            _target = _activeImage != null ? (Graphic)_activeImage : textMeshProUGUI;
            Material source = _activeImage != null ? (imageMaterial != null ? imageMaterial : _activeImage.material) : fontMaterial;
            if (_target == null || source == null || _canvasGroup == null ||
                !source.HasProperty(Bevel) || !source.HasProperty(LightAngle))
            {
                Debug.LogError("GameNameAnimation requires a CanvasGroup, a target and a material with _Bevel and _LightAngle.", this);
                enabled = false;
                return;
            }

            _runtimeFontMaterial = new Material(source);
            if (_activeImage != null)
            {
                if (imageEng != null)
                {
                    _originalEnglishMaterial = imageEng.material;
                    imageEng.material = _runtimeFontMaterial;
                }
                if (imageUkr != null && imageUkr != imageEng)
                {
                    _originalUkrainianMaterial = imageUkr.material;
                    imageUkr.material = _runtimeFontMaterial;
                }
            }
            else
            {
                _originalFontMaterial = textMeshProUGUI.fontSharedMaterial;
                textMeshProUGUI.fontSharedMaterial = _runtimeFontMaterial;
            }
            RefreshLanguage();
            
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private Image GetLanguageImage()
        {
            Image preferred = GameModel.CurrentLanguage == TranslatorLanguages.ENG ? imageEng : imageUkr;
            return preferred != null ? preferred : (imageUkr != null ? imageUkr : imageEng);
        }

        private void RefreshLanguage()
        {
            if (_runtimeFontMaterial == null) return;
            Image selected = GetLanguageImage();
            if (selected == null) return;

            // Preserve fade progress when the language changes during the animation.
            float alpha = _target.color.a;
            _activeImage = selected;
            _target = selected;
            SetAlpha(alpha);
            if (imageEng != null) imageEng.enabled = imageEng == selected;
            if (imageUkr != null) imageUkr.enabled = imageUkr == selected;
            if (textMeshProUGUI != null) textMeshProUGUI.enabled = false;
            SetLighting(_runtimeFontMaterial.GetFloat(Bevel), _runtimeFontMaterial.GetFloat(LightAngle));
        }

        private void SetAlpha(float alpha)
        {
            Color color = _target.color;
            color.a = alpha;
            _target.color = color;
        }

        private void OnDestroy()
        {
            if (_languageSubscription.HasValue)
                _translatorService.Unsubscribe(_languageSubscription.Value);

            if (_runtimeFontMaterial != null)
            {
                if (imageEng != null && imageEng.material == _runtimeFontMaterial)
                    imageEng.material = _originalEnglishMaterial;
                if (imageUkr != null && imageUkr != imageEng && imageUkr.material == _runtimeFontMaterial)
                    imageUkr.material = _originalUkrainianMaterial;

                if (textMeshProUGUI != null && textMeshProUGUI.fontSharedMaterial == _runtimeFontMaterial)
                    textMeshProUGUI.fontSharedMaterial = _originalFontMaterial;

                Destroy(_runtimeFontMaterial);
            }

            if (Inst == this) Inst = null;
        }

        public Coroutine StartAnima()
        {
            if (!isActiveAndEnabled || _runtimeFontMaterial == null) return null;
            StopAllCoroutines();
            RefreshLanguage();
            return StartCoroutine(GameNameRoutine());
        }

        private void SetLighting(float bevel, float angle)
        {
            _runtimeFontMaterial.SetFloat(Bevel, bevel);
            _runtimeFontMaterial.SetFloat(LightAngle, angle);
            // Mask creates a stencil material copy; animate that copy as well.
            if (_activeImage != null)
            {
                Material rendered = _activeImage.materialForRendering;
                rendered.SetFloat(Bevel, bevel);
                rendered.SetFloat(LightAngle, angle);
            }
        }

        private IEnumerator GameNameRoutine()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            _fadeService?.VisibleFade(false);
            SetLighting(0, 0);

            SetAlpha(0);
            for (float i = 0; i <= 1.0f; i += Time.deltaTime)
            {
                SetAlpha(i);
                yield return null;
            }

            SetAlpha(1);
            float angel = 0.0f;
            for (float i = 0; i <= 1.0f; i += Time.deltaTime)
            {
                angel += Time.deltaTime * 3;
                SetLighting(i, angel);

                yield return null;
            }

            for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime)
            {
                angel += Time.deltaTime * 3;
                SetLighting(i, angel);
                yield return null;
            }

            SetLighting(0, angel);
            for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime)
            {
                SetAlpha(i);
                yield return null;
            }

            SetAlpha(0);
            _fadeService?.VisibleFade(true);
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}
