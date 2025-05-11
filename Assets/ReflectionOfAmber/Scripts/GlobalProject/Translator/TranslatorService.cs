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
        private readonly CoroutineHelper m_coroutineHelper;
        private readonly GameResourcesService m_gameResourcesService;

        private static readonly Dictionary<string, TranslatorData> TranslatorData = new();

        private readonly Dictionary<uint, Sub> m_subscribers = new();
        private uint m_subsCount = 0;
        public event Action OnReady;

        private readonly List<IEnumerator> _loadList = new ();

        [Inject]
        public TranslatorService(CoroutineHelper coroutineHelper, GameResourcesService gameResourcesService)
        {
            m_coroutineHelper = coroutineHelper;
            m_gameResourcesService = gameResourcesService;
        }

        public void Init()
        {
            m_coroutineHelper.StartCoroutine(CheckConnectToTablets(GlobalConstant.ScenarioURL));
        }

        private IEnumerator CheckConnectToTablets(string url)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
            
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(unityWebRequest.error);

                TakeCashedText();
                
                OnReady?.Invoke();
            }
            else
            {
                _loadList.Add(LoadText(GlobalConstant.ScenarioURL));
                _loadList.Add(LoadText(GlobalConstant.OtherTextURL));

                m_coroutineHelper.StartCoroutine(_loadList[0]);
            }
        }

        private void TakeCashedText()
        {
            foreach (var localizationBlockData in m_gameResourcesService.LocalizationConfig.ScenarioLocalizationBlockData)
            {
                string[] langs = new string[localizationBlockData.LocalizationDatas.Length];
                string[] texts = new string[localizationBlockData.LocalizationDatas.Length];

                for (int i = 0; i < localizationBlockData.LocalizationDatas.Length; i++)
                {
                    langs[i] = localizationBlockData.LocalizationDatas[i].Language.ToString();
                    texts[i] = localizationBlockData.LocalizationDatas[i].Text;
                }

                TranslatorData translatorData = new TranslatorData(langs, texts);
                TranslatorData.Add(localizationBlockData.Key, translatorData);
            }
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
                m_coroutineHelper.StartCoroutine(_loadList[0]);
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