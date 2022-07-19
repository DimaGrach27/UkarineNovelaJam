using System;
using UnityEngine;

namespace GameScene.ChooseWindow.CameraAction
{
    public class CameraActionService
    {
        public event Action OnTakePhoto;
        
        private readonly CameraActionUiView _cameraActionUiView;
        
        public CameraActionService(Transform uiTransform)
        {
            _cameraActionUiView = uiTransform.GetComponentInChildren<CameraActionUiView>();
            _cameraActionUiView.OnTakePhoto += OnTakePhotoAction;

            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
            _cameraActionUiView.IsReadyToTakePhoto = SaveService.CameraFilmLeft > 0;
        }

        public void ChangeVisible(bool isVisible) => _cameraActionUiView.Visible = isVisible;

        private void OnTakePhotoAction()
        {
            SaveService.CameraFilmLeft--;
            _cameraActionUiView.FilmLeft = SaveService.CameraFilmLeft;
            _cameraActionUiView.IsReadyToTakePhoto = SaveService.CameraFilmLeft > 0;
            OnTakePhoto?.Invoke();
        }
    }
}