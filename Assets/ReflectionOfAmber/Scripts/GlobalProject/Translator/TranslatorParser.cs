using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    public class TranslatorParser
    {
        [Inject]
        public TranslatorParser()
        {
            string scenarioFile = Resources.Load<TextAsset>("Scenario").text;

            _scenarioTexts = new();
            
            string oneSep = $"{GlobalConstant.Koma}{GlobalConstant.DubbleComa}";
            string twoSep = $"{GlobalConstant.DubbleComa}{GlobalConstant.Koma}{GlobalConstant.DubbleComa}";

            string[] separators = { oneSep, twoSep };
            string[] rows = scenarioFile.Split('\n');
  
            string[] firstLine = rows[0].Split(',');
            
            int lineLength = firstLine.Length;

            string[] languages = new string[lineLength - 1];
            
            for (int i = 1; i < firstLine.Length; i++)
            {
                string text = firstLine[i];
                text = text.Replace(GlobalConstant.SymbolR.ToString(), "");
                text = text.Replace(GlobalConstant.SymbolR.ToString(), "");
                
                languages[i - 1] = text;
            }
            
            for (int i = 1; i < rows.Length; i++)
            {
                string[] lineArray = rows[i].Split(',');

                if (lineArray.Length > 3)
                {
                    lineArray = rows[i].Split(separators, StringSplitOptions.None);
                }
                
                string[] texts = new string[lineLength - 1];
                
                for (int j = 1; j < lineLength; j++)
                {
                    texts[j - 1] = lineArray[j];
                }

                TranslatorData translatorData = new TranslatorData(languages, texts);
                
                string text = lineArray[0];
                text = text.Replace(GlobalConstant.SymbolR.ToString(), "");
                text = text.Replace(GlobalConstant.SymbolR.ToString(), "");
                
                _scenarioTexts.Add(text, translatorData);
            }
        }

        private readonly Dictionary<string, TranslatorData> _scenarioTexts;

        public string GetText(string key, string lang)
        {
            string text = null;

            if (_scenarioTexts.ContainsKey(key))
                text = _scenarioTexts[key].GetText(lang);

            return text;
        }
    }
}