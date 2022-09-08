using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowSaveScreenPart : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TextMeshProUGUI descText;

        public event Action<int> OnCLickButton;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnCLickButtonHandler);
        }

        public int Index { get; set; }

        public Sprite Sprite
        {
            set => image.sprite = value;
        }

        public bool IsHaveSave
        {
            set => image.color = value ? Color.white : Color.black;
        }

        public string Description
        {
            set => descText.text = value;
        }

        private void OnCLickButtonHandler() => OnCLickButton?.Invoke(Index);
    }
}