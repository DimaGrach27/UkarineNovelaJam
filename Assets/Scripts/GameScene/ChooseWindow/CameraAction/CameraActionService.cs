using System;
using UnityEngine;

namespace GameScene.ChooseWindow.CameraAction
{
    public class CameraActionService
    {
        public event Action OnTakePhoto;
        
        private readonly CameraActionUiView _cameraActionUiView;
        private readonly CameraActionFlash _cameraActionFlash;
        
        public CameraActionService(Transform uiTransform)
        {
            _cameraActionUiView = uiTransform.GetComponentInChildren<CameraActionUiView>();
            _cameraActionUiView.OnTakePhoto += OnTakePhotoAction;
            
            _cameraActionFlash = uiTransform.GetComponentInChildren<CameraActionFlash>();;

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
    }
}