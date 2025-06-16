using System;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.PreInitScene
{
    public class GameIniter
    {
        [Inject]
        public GameIniter(List<IInit> inits)
        {
            m_Inits = inits;
        }

        private readonly List<IInit> m_Inits;
        public event Action OnInitEnded;
        
        public void Initialize()
        {
            Debug.Log($"Data saved path: {Application.persistentDataPath}");

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
            
            
            OnInitEnded?.Invoke();
        }
    }
}