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
        private const string KEY_FIRST_LOAD = "first_load";
        
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
            bool isWasLoad = PlayerPrefs.GetInt(KEY_FIRST_LOAD, 0) == 1;

            if (isWasLoad)
            {
                LoadMineMenu();
                return;
            }
            
            settingElementSlider.OnChangeValue += OnChangeValue;
            settingElementSlider.SetValue(SaveService.GetBrightnessValue() * 10);
            loadButton.onClick.AddListener(LoadMineMenu);
        }

        private void OnChangeValue(float value)
        {
            _globalBrightnessService.BrightnessValue = (value / 10);
            SaveService.SaveBrightnessValue(value / 10);
        }

        private void LoadMineMenu()
        {
            PlayerPrefs.SetInt(KEY_FIRST_LOAD, 1);
            SceneManager.LoadScene("MainMenu");
        }
    }
}