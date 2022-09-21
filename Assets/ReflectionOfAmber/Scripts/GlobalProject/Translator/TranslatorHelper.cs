using TMPro;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TranslatorHelper : MonoBehaviour
    {
        [SerializeField] private TranslatorKeys translatorKey;

        private void Start()
        {
            GetComponent<TextMeshProUGUI>().text =
                TranslatorParser.GetText(translatorKey.ToString(), TranslatorLanguages.ENG.ToString());
        }
    }
}