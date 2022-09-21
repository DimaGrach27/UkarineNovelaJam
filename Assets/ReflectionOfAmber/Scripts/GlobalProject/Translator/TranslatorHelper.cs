using ReflectionOfAmber.Scripts.GameModelBlock;
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
        public void Construct(TranslatorParser translatorParser)
        {
            _translatorParser = translatorParser;
            _translatorParser.OnReady += UpdateText;
        }

        private TranslatorParser _translatorParser;

        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text =
                TranslatorParser.GetText(translatorKey.ToString(), GameModel.CurrentLanguage);
        }

        private void UpdateText()
        {
            _translatorParser.OnReady -= UpdateText;
            _translatorParser = null;
            
            GetComponent<TextMeshProUGUI>().text =
                TranslatorParser.GetText(translatorKey.ToString(), GameModel.CurrentLanguage);
        }
    }
}