using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameModelBlock;
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
            _inits = inits;
            _loadingScreenView = loadingScreenView;
        }

        private readonly List<IInit> _inits;
        private readonly LoadingScreenView _loadingScreenView;
        
        public void Initialize()
        {
            if (_inits.Count > 0)
            {
                _inits[0].OnReady += InitNext;
                _inits[0].Init();
            }
        }

        private void InitNext()
        {
            _inits[0].OnReady -= InitNext;
            _inits.RemoveAt(0);
            
            if (_inits.Count > 0)
            {
                _inits[0].OnReady += InitNext;
                _inits[0].Init();
                return;
            }
            

            if (SaveService.BrightnessStatus)
            {
                LoadMineMenu();
            }
            else
            {
                Object.Destroy(_loadingScreenView.gameObject);
            }
        }
        
        private void LoadMineMenu()
        {
            SaveService.BrightnessStatus = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}