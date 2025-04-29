using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    public class TranslatorService : IInit
    {
        private const string Id = "1ym156FGXOVntcnxxydhQx8hRfOE5EzgpoxMXq53fCbc";
        private const string ExportFormat = "export?format=tsv";
        private const string GidScenario = "327397956"; // 0 - is old scenario, 327397956 - new scenario (parsed)
        private const string GidOtherText = "208247162";
        // private static readonly string ScenarioURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}";
        private static readonly string ScenarioURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}&id={Id}&gid={GidScenario}";
        private static readonly string OtherTextURL = $"https://docs.google.com/spreadsheets/d/{Id}/{ExportFormat}&id={Id}&gid={GidOtherText}";
        
        private readonly CoroutineHelper _coroutineHelper;
        
        private static readonly Dictionary<string, TranslatorData> TranslatorData = new();

        private readonly Dictionary<uint, Sub> m_subscribers = new();
        private uint m_subsCount = 0;
        public event Action OnReady;
        // public event Action OnLanguageChanged;

        private readonly List<IEnumerator> _loadList = new ();

        [Inject]
        public TranslatorService(CoroutineHelper coroutineHelper)
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

            string[] rows = scenarioFile.Split($"{'\r'}{'\n'}");
            string[] firstLine = rows[0].Split('\t');
            
            int lineLength = firstLine.Length;

            string[] languages = new string[lineLength - 1];
            
            for (int i = 1; i < firstLine.Length; i++)
            {
                string text = firstLine[i];

                languages[i - 1] = text;
            }
            
            for (int i = 1; i < rows.Length; i++)
            {
                string[] lineArray = rows[i].Split('\t');
                string[] texts = new string[lineLength - 1];
                
                for (int j = 1; j < lineLength; j++)
                {
                    texts[j - 1] = lineArray[j];
                }

                TranslatorData translatorData = new TranslatorData(languages, texts);
                
                string text = lineArray[0];

                TranslatorData.Add(text, translatorData);
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

        public static string GetText(string key)
        {
            if (!TranslatorData.TryGetValue(key, out TranslatorData value))
            {
                return String.Empty;
            }
            
            string text = value.GetText(GameModel.CurrentLanguage.ToString());

            if (string.IsNullOrEmpty(text))
            {
                text = value.GetText(TranslatorLanguages.UKR.ToString());
            }

            return text;
        }
        
        public static string GetText(TranslatorKeys key)
        {
            return GetText(key.ToString());
        }

        public uint Subscribe(object obj, Action callback)
        {
            Sub sub = new Sub(obj, callback);
 
            if (!m_subscribers.TryAdd(m_subsCount, sub))
            {
            }

            return m_subsCount++;
        }

        public uint Subscribe(MonoBehaviour obj, Action callback)
        {
            Sub sub = new Sub(obj, callback);

            if (!m_subscribers.TryAdd(m_subsCount, sub))
            {
                
            }
            return m_subsCount++;
        }
        
        public void Unsubscribe(uint index)
        {
            if (!m_subscribers.ContainsKey(index))
            {
                Debug.LogWarning($"{index} don't present in map!");
                return;
            }
            m_subscribers.Remove(index);
        }
        
        public void ChangeLanguage(TranslatorLanguages translatorLanguages)
        {
            if (translatorLanguages != GameModel.CurrentLanguage)
            {
                SaveService.LanguageStatus = translatorLanguages;
            }
            
            GameModel.CurrentLanguage = translatorLanguages;

            List<uint> invalidIndexes = new();
            foreach (var keyVal in m_subscribers)
            {
                try
                {
                    if (keyVal.Value.Obj != null)
                    {
                        keyVal.Value.Callback?.Invoke();
                    }
                }
                catch (MissingReferenceException missRef)
                {
                    invalidIndexes.Add(keyVal.Key);
                }
            }

            foreach (var index in invalidIndexes)
            {
                Unsubscribe(index);
            }
        }

        class Sub
        {
            public readonly object Obj;
            public Action Callback;

            public Sub(object obj, Action callback)
            {
                Obj = obj;
                Callback = callback;
            }
        }
    }
}