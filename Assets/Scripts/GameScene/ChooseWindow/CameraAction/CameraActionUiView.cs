using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.ChooseWindow.CameraAction
{
    public class CameraActionUiView : MonoBehaviour
    {
        public event Action OnTakePhoto;
        
        private Button _button;
        private Button Button => _button ??= GetComponent<Button>();

        [SerializeField] private TextMeshProUGUI countText;
        
        private void Awake()
        {
            Button.onClick.AddListener(TakePhoto);
        }

        public int FilmLeft
        {
            set => countText.text = value.ToString();
        }

        private bool _isReadyToTakePhoto;
        public bool IsReadyToTakePhoto
        {
            set
            {
                _isReadyToTakePhoto = value;
                Button.interactable = _isReadyToTakePhoto;
            }
            get => _isReadyToTakePhoto;
        }

        public bool Visible
        {
            set => gameObject.SetActive(value);
        }

        private void TakePhoto()
        {
            if(!IsReadyToTakePhoto) return;
            
            OnTakePhoto?.Invoke();
        }
    }
}