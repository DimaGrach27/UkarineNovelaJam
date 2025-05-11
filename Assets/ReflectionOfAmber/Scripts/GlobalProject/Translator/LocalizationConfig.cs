using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ReflectionOfAmber.Scripts.GlobalProject.Translator
{
    [CreateAssetMenu(fileName = "new Localization config", menuName = "Frog Croaked Team/Create 'Localization config'", order = 0)]
    public class LocalizationConfig : ScriptableObject
    {
        public List<LocalizationBlockData> ScenarioLocalizationBlockData;
        public List<LocalizationBlockData> TextsLocalizationBlockData;
        
        [ContextMenu("Update Localization")]
        public void UpdateLocalization()
        {
            LoadText(GlobalConstant.ScenarioURL, ScenarioLocalizationBlockData);
            LoadText(GlobalConstant.OtherTextURL, TextsLocalizationBlockData);
        }
        
        private async void LoadText(string urlLoad, List<LocalizationBlockData> localizationBlockDatas)
        {
            localizationBlockDatas.Clear();
            
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(urlLoad);
            
            await unityWebRequest.SendWebRequest();

            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(unityWebRequest.error);
                return;
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

            // localizationBlockDatas = new LocalizationBlockData[rows.Length - 1];
            
            for (int i = 1; i < rows.Length; i++)
            {
                string[] lineArray = rows[i].Split('\t');
                string[] texts = new string[lineLength - 1];
                
                LocalizationData[] localizationDatas = new LocalizationData[languages.Length];

                for (int j = 1; j < lineLength; j++)
                {
                    texts[j - 1] = lineArray[j];
                    localizationDatas[j - 1] = new LocalizationData()
                    {
                        Language = GetTranslationLanguageByString(languages[j - 1]),
                        Text = lineArray[j]
                    };
                }

                string key = lineArray[0];
                
                LocalizationBlockData localizationBlockData = new LocalizationBlockData()
                {
                    Key = key,
                    LocalizationDatas = localizationDatas
                };
                
                localizationBlockDatas.Add(localizationBlockData);
                // localizationBlockDatas[i - 1] = localizationBlockData;
            }
            
            Debug.Log($"Success update list from {urlLoad}");
        }

        private TranslatorLanguages GetTranslationLanguageByString(string str)
        {
            switch (str)
            {
                case "UKR":
                    return TranslatorLanguages.UKR;
                case "ENG":
                    return TranslatorLanguages.ENG;
            }

            throw new Exception($"Unknown language '{str}'");
        }
    }

    [Serializable]
    public class LocalizationData
    {
        public TranslatorLanguages Language;
        public string Text;
    }
    
    [Serializable]
    public class LocalizationBlockData
    {
        public string Key;
        public LocalizationData[] LocalizationDatas;
    }
}