using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.Authenticator;
using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.Steam
{
    public class SteamTestExample : MonoBehaviour
    {
        private UserUnityService m_UserUnityService;
        private SteamService m_SteamService;
        private AuthenticatorService m_AuthenticatorService;

        private Queue<IInit> m_Inits;

        private string filePath = "test_file_path.txt";
        public string fileData = "";

        public AchievementKeys achievementKey;

        private void Awake()
        {
            m_UserUnityService = new ();
            m_SteamService = new ();
            m_AuthenticatorService = new ();

            m_Inits = new Queue<IInit>();
            m_Inits.Enqueue(m_UserUnityService);
            m_Inits.Enqueue(m_SteamService);
            m_Inits.Enqueue(m_AuthenticatorService);
        }

        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            if (m_Inits.Count > 0)
            {
                Debug.Log($"Init: {m_Inits.Peek().GetType().Name}");
                m_Inits.Peek().OnReady += InitNext;
                m_Inits.Peek().Init();
            }
        }

        private void InitNext()
        {
            m_Inits.Peek().OnReady -= InitNext;
            m_Inits.Dequeue();
            
            if (m_Inits.Count > 0)
            {
                Debug.Log($"Init: {m_Inits.Peek().GetType().Name}");
                m_Inits.Peek().OnReady += InitNext;
                m_Inits.Peek().Init();
                return;
            }
            
            Debug.Log($"EndInit");
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