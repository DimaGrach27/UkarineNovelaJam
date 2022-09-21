using System.Collections.Generic;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    public class TranslatorData
    {
        public TranslatorData(string[] lang, string[] texts)
        {
            _dictionary = new();
            
            for (int i = 0; i < lang.Length; i++)
            {
                string text = texts[i];
                _dictionary.Add(lang[i], text);
            }
        }
        
        private readonly Dictionary<string, string> _dictionary;

        public string GetText(string lang)
        {
            string text = null;

            if (_dictionary.ContainsKey(lang))
                text = _dictionary[lang];

            return text;
        }
    }
}