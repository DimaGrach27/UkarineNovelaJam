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
                if (m_textMesh == null)
                {
                    m_textMesh = GetComponent<TextMeshProUGUI>();
                }

                return m_textMesh;
            }
        }

        [Inject]
        public void Construct(TranslatorService translatorService)
        {
            Unsubscribe();
            m_translatorService = translatorService;
            m_translatorService.OnReady += OnTranslatorReady;
            m_subIndex = m_translatorService.Subscribe(this, UpdateText);
        }

        private TranslatorService m_translatorService;
        private uint? m_subIndex;

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
            m_translatorService.OnReady -= OnTranslatorReady;

            UpdateText();
        }

        private void UpdateText()
        {
            // Inactive scene objects can be destroyed without receiving OnDestroy.
            if (this == null)
            {
                Unsubscribe();
                return;
            }

            var textMesh = TextMesh;
            if (textMesh == null)
            {
                Debug.LogError("TranslatorHelper requires a TextMeshProUGUI component.", this);
                return;
            }
            
            textMesh.text = TranslatorService.GetText(translatorKey);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Unsubscribe()
        {
            if (m_translatorService == null)
            {
                return;
            }

            m_translatorService.OnReady -= OnTranslatorReady;
            if (m_subIndex.HasValue)
            {
                m_translatorService.Unsubscribe(m_subIndex.Value);
                m_subIndex = null;
            }
        }
    }
}
