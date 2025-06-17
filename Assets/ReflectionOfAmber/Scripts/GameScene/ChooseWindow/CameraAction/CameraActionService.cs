using System;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.Input;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction
{
    public class CameraActionService : IDisposable, IInputListener
    {
        private readonly InputService m_InputService;
        public event Action OnTakePhoto;
        
        private readonly CameraActionUiView _cameraActionUiView;
        private readonly CameraActionFlash _cameraActionFlash;
        
        private bool m_isActive;

        [Inject]
        public CameraActionService(
            GamePlayCanvas gamePlayCanvas,
            InputService inputService
            )
        {
            m_InputService = inputService;
            _cameraActionUiView = gamePlayCanvas.GetComponentInChildren<CameraActionUiView>();
            _cameraActionFlash = gamePlayCanvas.GetComponentInChildren<CameraActionFlash>();;
            
            _cameraActionUiView.OnTakePhoto += OnTakePhotoAction;

            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
            _cameraActionUiView.IsReadyToTakePhoto = SaveService.CameraFilmLeft > 0;
            
            m_InputService.AddListener(this);
        }

        public void SetActive(bool isActive)
        {
            m_isActive = isActive;
            _cameraActionUiView.Visible = isActive;
        }

        private void OnTakePhotoAction()
        {
            Debug.Log("Take photo fff");
            
            SaveService.CameraFilmLeft--;
            
            _cameraActionFlash.CallFlash();

            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
            _cameraActionUiView.IsReadyToTakePhoto = SaveService.CameraFilmLeft > 0;
            OnTakePhoto?.Invoke();
        }
        
        public void TakePhotoAction()
        {
            _cameraActionFlash.CallFlash();
            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
        }

        public void Dispose()
        {
            _cameraActionUiView.OnTakePhoto -= OnTakePhotoAction;
            m_InputService.RemoveListener(this);
        }

        public void OnInputAction(InputActionEnum inputActionEnum)
        {
            if (inputActionEnum == InputActionEnum.CAMERA_ACTION && m_isActive)
            {
                OnTakePhotoAction();
            }
        }

        public bool ShouldReceiveInput { get; set; } = true;
    }
}