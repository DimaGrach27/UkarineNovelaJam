using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    public class TranslatorParser : IInitializable
    {
        private const string ScenarioURL = "https://docs.google.com/spreadsheets/d/1ym156FGXOVntcnxxydhQx8hRfOE5EzgpoxMXq53fCbc/export?format=csv";
        
        private readonly CoroutineHelper _coroutineHelper;
        
        private readonly Dictionary<string, TranslatorData> _scenarioTexts = new();
        
        [Inject]
        public TranslatorParser(CoroutineHelper coroutineHelper)
        {
            _coroutineHelper = coroutineHelper;
        }
        
        public void Initialize()
        {
            _coroutineHelper.StartCoroutine(LoadText());
        }

        private IEnumerator LoadText()
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(ScenarioURL);
            
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(unityWebRequest.error);
                yield break;
            }
            
            string scenarioFile = unityWebRequest.downloadHandler.text;

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

        public string GetText(string key, string lang)
        {
            string text = null;

            if (_scenarioTexts.ContainsKey(key))
                text = _scenarioTexts[key].GetText(lang);

            return text;
        }
    }
}