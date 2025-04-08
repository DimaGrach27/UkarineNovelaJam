using System;
using System.IO;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.Steam
{
    public class SteamService : IInit, ITickable, IDisposable
    {
#if GAME_DEMO
        public const int APP_ID = 3580180;
#else
        public const int APP_ID = 3562420;
#endif
        
        public event Action OnReady;
        
        private const string LANG_SET_KEY = "lang_was_set_key";
        
        public void Init()
        {
            try
            {
                SteamClient.Init(APP_ID);
                string steamName = SteamClient.Name;
                Debug.Log($"Steam name is = {steamName}"); 

                CheckLang();
                
                Debug.Log($"Files saved in cloud: ");
                foreach (var file in SteamRemoteStorage.Files)
                {
                    string jsonFile = LoadFileFromCloud(file);
                    string filePath = SaveService.Path(file);
                    
                    File.WriteAllText(filePath, jsonFile);
                    Debug.Log( $"{file} ({SteamRemoteStorage.FileSize(file)} {SteamRemoteStorage.FileTime(file)})" );
                }
                
                OnReady?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                // Something went wrong - it's one of these:
                //
                //     Steam is closed?
                //     Can't find steam_api dll?
                //     Don't have permission to play app?
                //
                throw;
            }
        }
        
        public void Tick()
        {
            SteamClient.RunCallbacks();
        }

        public void CompleteAchievement(AchievementKeys achieveKey)
        {
#if !GAME_DEMO
            var ach = new Achievement(SteamAchievementKeys.GetId(achieveKey));
            ach.Trigger();
            
            SteamUserStats.StoreStats();
#endif
        }

        public void ClearAchievement(AchievementKeys achieveKey)
        {
            var ach = new Achievement(SteamAchievementKeys.GetId(achieveKey));
            ach.Clear();
        }
        
        public void SaveFileToCloud(string data, string filename)
        {
            if (string.IsNullOrEmpty(data) || string.IsNullOrEmpty(filename))
            {
                Debug.LogError("Invalid data and filename provided to save file");
                return;
            }
            
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);

            SteamRemoteStorage.FileWrite(filename, bytes);
            
            Debug.Log("Saved files: ");
            foreach ( var file in SteamRemoteStorage.Files )
            {
                Debug.Log( $"{file} ({SteamRemoteStorage.FileSize(file)} {SteamRemoteStorage.FileTime( file )})" );
            }
        }

        public void DeleteFileFromCloud(string filename)
        {
            SteamRemoteStorage.FileDelete(filename);
        }
        
        public string LoadFileFromCloud(string filename)
        {
            byte[] bytes = SteamRemoteStorage.FileRead(filename);

            int bytesLength = bytes.Length;

            string result = System.Text.Encoding.UTF8.GetString(bytes, 0, bytesLength);
            
            return result;
        }

        private void CheckLang()
        {
            if (PlayerPrefs.HasKey(LANG_SET_KEY))
            {
                return;
            }
            
            string lang = SteamApps.GameLanguage;
            
            Debug.Log($"Steam language is = {lang}");

            switch (lang)
            {
                case "ukrainian":
                    SaveService.LanguageStatus = TranslatorLanguages.UKR;
                    break;
                case "english":
                    SaveService.LanguageStatus = TranslatorLanguages.ENG;
                    break;
                
                default:
                    SaveService.LanguageStatus = TranslatorLanguages.ENG;
                    break;
            }
            
            PlayerPrefs.SetInt(LANG_SET_KEY, 1);
        }
        
        public void Dispose()
        {
            SteamClient.Shutdown();
        }
    }
}