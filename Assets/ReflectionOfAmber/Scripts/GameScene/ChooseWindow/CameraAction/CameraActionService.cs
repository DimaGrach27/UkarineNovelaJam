using System;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ChooseWindow.CameraAction
{
    public class CameraActionService : IDisposable
    {
        public event Action OnTakePhoto;
        
        private readonly CameraActionUiView _cameraActionUiView;
        private readonly CameraActionFlash _cameraActionFlash;

        [Inject]
        public CameraActionService(GamePlayCanvas gamePlayCanvas)
        {
            _cameraActionUiView = gamePlayCanvas.GetComponentInChildren<CameraActionUiView>();
            _cameraActionFlash = gamePlayCanvas.GetComponentInChildren<CameraActionFlash>();;
            
            _cameraActionUiView.OnTakePhoto += OnTakePhotoAction;

            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
            _cameraActionUiView.IsReadyToTakePhoto = SaveService.CameraFilmLeft > 0;
        }

        public void ChangeVisible(bool isVisible) => _cameraActionUiView.Visible = isVisible;

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
            Debug.Log("CameraActionService Disposing");
        }
    }
}