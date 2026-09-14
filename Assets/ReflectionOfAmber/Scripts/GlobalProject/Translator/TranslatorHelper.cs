using TMPro;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TranslatorHelper : MonoBehaviour
    {
        [SerializeField] private TranslatorKeys translatorKey;
        
        private TextMeshProUGUI m_textMesh;

        private TextMeshProUGUI TextMesh
        {
            get
            {
                return m_textMesh ??= GetComponent<TextMeshProUGUI>();
            }
        }

        [Inject]
        public void Construct(TranslatorService translatorService)
        {
            m_translatorService = translatorService;
            m_translatorService.OnReady += OnTranslatorReady;
            m_subIndex = m_translatorService.Subscribe(this, UpdateText);
        }

        private TranslatorService m_translatorService;
        private uint m_subIndex;

        private void Awake()
        {
            m_textMesh = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            UpdateText();
        }

        private void OnTranslatorReady()
        {
            m_translatorService.OnReady -= UpdateText;

            UpdateText();
        }

        private void UpdateText()
        {
            if (TextMesh is null)
            {
                Debug.LogError($"{transform.parent.name} = m_textMesh is null");
                return;
            }
            
            TextMesh.text = TranslatorService.GetText(translatorKey);
        }

        private void OnDestroy()
        {
            m_translatorService.OnReady -= OnTranslatorReady;
            m_translatorService.Unsubscribe(m_subIndex);
        }
    }
}