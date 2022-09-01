using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ReflectionOfAmber.Scripts.PreInitScene
{
    public class BrightnessService : MonoBehaviour
    {
        [SerializeField] private SettingElementSlider settingElementSlider;
        [SerializeField] private Button loadButton;
        
        private GlobalBrightnessService _globalBrightnessService;

        [Inject]
        public void Construct(GlobalBrightnessService globalBrightnessService)
        {
            _globalBrightnessService = globalBrightnessService;
        }
        
        private void Awake()
        {
            bool isWasLoad = SaveService.BrightnessStatus;

            if (isWasLoad)
            {
                LoadMineMenu();
                return;
            }
            
            settingElementSlider.OnChangeValue += OnChangeValue;
            settingElementSlider.SetValue(SaveService.BrightnessValue * 10);
            loadButton.onClick.AddListener(LoadMineMenu);
        }

        private void OnChangeValue(float value)
        {
            _globalBrightnessService.BrightnessValue = (value / 10);
            SaveService.BrightnessValue = value / 10;
        }

        private void LoadMineMenu()
        {
            SaveService.BrightnessStatus = true;
            SceneManager.LoadScene("MainMenu");
        }
    }
}