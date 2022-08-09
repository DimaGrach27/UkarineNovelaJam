using System.Collections;
using TMPro;
using UnityEngine;

namespace ReflectionOfAmber.Scripts
{
    public class GameNameAnimation : MonoBehaviour
    {
        public static GameNameAnimation Inst { get; private set; }

        [SerializeField] private Material fontMaterial;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private CanvasGroup _canvasGroup;

        private static readonly int Bevel = Shader.PropertyToID("_Bevel");
        private static readonly int LightAngle = Shader.PropertyToID("_LightAngle");

        private void Awake()
        {
            if (Inst != null) return;

            Inst = this;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public Coroutine StartAnima() => StartCoroutine(GameNameRoutine());

        private IEnumerator GameNameRoutine()
        {
            _canvasGroup.alpha = 1.0f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            FadeService.FadeService.VisibleFade(false);

            Color colorText = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            for (float i = 0; i <= 1.0f; i += Time.deltaTime)
            {
                colorText.a = i;
                textMeshProUGUI.color = colorText;
                yield return null;
            }

            float angel = 0.0f;
            for (float i = 0; i <= 1.0f; i += Time.deltaTime)
            {
                angel += Time.deltaTime * 3;
                fontMaterial.SetFloat(Bevel, i);
                fontMaterial.SetFloat(LightAngle, angel);

                yield return null;
            }

            for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime)
            {
                angel += Time.deltaTime * 3;
                fontMaterial.SetFloat(Bevel, i);
                fontMaterial.SetFloat(LightAngle, angel);
                yield return null;
            }

            for (float i = 1.0f; i >= 0.0f; i -= Time.deltaTime)
            {
                colorText.a = i;
                textMeshProUGUI.color = colorText;
                yield return null;
            }

            FadeService.FadeService.VisibleFade(true);
            _canvasGroup.alpha = 0.0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}