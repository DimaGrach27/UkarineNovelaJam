using System;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Steam
{
    public class SteamTestExample : MonoBehaviour
    {
        private SteamService m_SteamService;

        private string filePath = "test_file_path.txt";
        public string fileData = "";

        public AchievementKeys achievementKey;
        
        private void Awake()
        {
            m_SteamService = new SteamService();
        }

        private void Start()
        {
            m_SteamService.Init();
        }

        private void Update()
        {
            m_SteamService?.Tick();
        }
        
        [ContextMenu("Make achievement")]
        private void MakeAchievement()
        {
            m_SteamService?.CompleteAchievement(achievementKey);
        }
        
        [ContextMenu("Clear achievement")]
        private void ClearAchievement()
        {
            m_SteamService?.ClearAchievement(achievementKey);
        }

        [ContextMenu("Load")]
        private void LoadFile()
        {
            fileData = m_SteamService?.LoadFileFromCloud(filePath);
        }
        
        [ContextMenu("Save")]
        private void SaveFile()
        {
            fileData += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] ";
            
            m_SteamService?.SaveFileToCloud(fileData, filePath);
        }
    }
}