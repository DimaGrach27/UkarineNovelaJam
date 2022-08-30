using System;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart
{
    [RequireComponent(typeof(Button))]
    public class ScreenPartNextDialogButton : MonoBehaviour
    {
        public event Action OnClickButton;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClickButtonHandler);
        }

        private void OnClickButtonHandler() => OnClickButton?.Invoke();
    }
}