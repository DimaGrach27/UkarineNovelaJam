using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindow
{
    public class NoteButtonUiView : MonoBehaviour
    {
        public event Action<int> OnChoose;
        
        [SerializeField] private TextMeshProUGUI textCount;
        
        private Button _button;

        private int _index = -1;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ClickButton);
        }

        public void InitButton(int index)
        {
            _index = index;
        }
        
        public void UpdateButton(int count)
        {
            textCount.text = count.ToString();
        }
        
        private void ClickButton()
        {
            OnChoose?.Invoke(_index);
        }
        
        public bool Visible
        {
            set => gameObject.SetActive(value);
        }
    }
}