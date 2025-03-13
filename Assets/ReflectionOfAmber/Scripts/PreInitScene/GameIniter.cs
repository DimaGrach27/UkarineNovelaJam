using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ReflectionOfAmber.Scripts.PreInitScene
{
    public class GameIniter : IInitializable
    {
        [Inject]
        public GameIniter(List<IInit> inits, LoadingScreenView loadingScreenView)
        {
            m_Inits = inits;
            m_LoadingScreenView = loadingScreenView;
        }

        private readonly List<IInit> m_Inits;
        private readonly LoadingScreenView m_LoadingScreenView;
        
        public void Initialize()
        {
            if (m_Inits.Count > 0)
            {
                Debug.Log($"Init: {m_Inits[0].GetType().Name}");
                m_Inits[0].OnReady += InitNext;
                m_Inits[0].Init();
            }
        }

        private void InitNext()
        {
            m_Inits[0].OnReady -= InitNext;
            m_Inits.RemoveAt(0);
            
            if (m_Inits.Count > 0)
            {
                Debug.Log($"Init: {m_Inits[0].GetType().Name}");
                m_Inits[0].OnReady += InitNext;
                m_Inits[0].Init();
                return;
            }
            
            if (SaveService.BrightnessStatus)
            {
                LoadMineMenu();
            }
            else
            {
                Object.Destroy(m_LoadingScreenView.gameObject);
            }
        }
        
        private void LoadMineMenu()
        {
            SaveService.BrightnessStatus = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}