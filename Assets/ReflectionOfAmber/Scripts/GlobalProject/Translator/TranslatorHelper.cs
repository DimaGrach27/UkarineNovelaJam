using TMPro;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TranslatorHelper : MonoBehaviour
    {
        [SerializeField] private TranslatorKeys translatorKey;

        [Inject]
        public void Construct(TranslatorService translatorService)
        {
            m_translatorService = translatorService;
            m_translatorService.OnReady += OnTranslatorReady;
            m_subIndex = m_translatorService.Subscribe(this, UpdateText);
        }

        private TranslatorService m_translatorService;
        private uint m_subIndex;

        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text =
                TranslatorService.GetText(translatorKey);
        }

        private void OnTranslatorReady()
        {
            m_translatorService.OnReady -= UpdateText;
            
            GetComponent<TextMeshProUGUI>().text =
                TranslatorService.GetText(translatorKey);
        }

        private void UpdateText()
        {
            GetComponent<TextMeshProUGUI>().text =
                TranslatorService.GetText(translatorKey);
        }

        private void OnDestroy()
        {
            m_translatorService.OnReady -= OnTranslatorReady;
            m_translatorService.Unsubscribe(m_subIndex);
        }
    }
}