using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    public class TranslatorParser : IInit
    {
        private const string Id = "1ym156FGXOVntcnxxydhQx8hRfOE5EzgpoxMXq53fCbc";
        private static readonly string ScenarioURL = $"https://docs.google.com/spreadsheets/d/{Id}/export?format=csv";
        private static readonly string OtherTextURL = $"https://docs.google.com/spreadsheets/d/{Id}/export?format=csv&id={Id}&gid=208247162";
        
        private readonly CoroutineHelper _coroutineHelper;
        
        private static readonly Dictionary<string, TranslatorData> TranslatorDatas = new();
        
        public event Action OnReady;

        private readonly List<IEnumerator> _loadList = new ();

        [Inject]
        public TranslatorParser(CoroutineHelper coroutineHelper)
        {
            _coroutineHelper = coroutineHelper;
        }

        public void Init()
        {
            _loadList.Add(LoadText(ScenarioURL));
            _loadList.Add(LoadText(OtherTextURL));

            _coroutineHelper.StartCoroutine(_loadList[0]);
        }

        private IEnumerator LoadText(string urlLoad)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(urlLoad);
            
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
                
                TranslatorDatas.Add(text, translatorData);
            }

            InitReady();
        }

        private void InitReady()
        {
            _loadList.RemoveAt(0);

            if (_loadList.Count > 0)
            {
                _coroutineHelper.StartCoroutine(_loadList[0]);
                return;
            }

            OnReady?.Invoke();
        }

        public static string GetText(string key, string lang)
        {
            string text = null;

            if (TranslatorDatas.ContainsKey(key))
                text = TranslatorDatas[key].GetText(lang);

            return text;
        }
    }
}