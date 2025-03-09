using System;
using System.IO;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using Steamworks;
using Steamworks.Data;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.Steam
{
    public class SteamService : IInit, ITickable, IDisposable
    {
        private const int AppID = 3562420;
        
        public event Action OnReady;
        
        public void Init()
        {
            try
            {
                SteamClient.Init(AppID);
                string steamName = SteamClient.Name;
                Debug.Log($"Steam name is = {steamName}"); 
                Debug.Log($"Steam language is = {SteamApps.GameLanguage}");
                
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
            var ach = new Achievement(SteamAchievementKeys.GetId(achieveKey));
            ach.Trigger();
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
        
        public void Dispose()
        {
            SteamClient.Shutdown();
        }
    }
}