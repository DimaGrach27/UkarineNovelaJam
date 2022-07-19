using System;
using System.Collections;
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
        
        [SerializeField] private Image cameraFlash;
        [SerializeField] private AnimationCurve flashCurve;


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
            StartCoroutine(FlashRoutine());
        }
        
        private IEnumerator FlashRoutine()
        {
            Color imageColor = GlobalConstant.ColorWitheClear;
            cameraFlash.gameObject.SetActive(true);
            cameraFlash.color = imageColor;
            
            float time = 0.0f;

            while (time < GlobalConstant.CAMERA_ACTION_FLASH_DURATION)
            {
                float alpha = flashCurve.Evaluate(CalculateProgress());
                time += Time.deltaTime;
                
                imageColor.a = alpha;
                cameraFlash.color = imageColor;
                
                yield return null;
            }
            
            cameraFlash.gameObject.SetActive(false);

            float CalculateProgress()
            {
                float progress = time / GlobalConstant.CAMERA_ACTION_FLASH_DURATION;
                return progress;
            }
        }
    }
}