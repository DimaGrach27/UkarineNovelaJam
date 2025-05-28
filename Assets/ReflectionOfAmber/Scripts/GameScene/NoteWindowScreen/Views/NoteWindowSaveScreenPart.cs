using System;
using ReflectionOfAmber.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowSaveScreenPart : MonoBehaviour
    {
        [SerializeField] 
        private Image image;
        [SerializeField] 
        private TextMeshProUGUI descText;

        private ButtonExt m_ButtonExt;

        public event Action<int> OnCLickButton;

        private void Awake()
        {
            m_ButtonExt = GetComponent<ButtonExt>();
            m_ButtonExt.onClick.AddListener(OnCLickButtonHandler);
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

        public Selectable GetSelectable
        {
            get => m_ButtonExt;
        }

        private void OnCLickButtonHandler() => OnCLickButton?.Invoke(Index);
    }
}