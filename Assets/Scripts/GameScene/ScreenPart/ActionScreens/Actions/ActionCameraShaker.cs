using DG.Tweening;
using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCameraShaker : IActionScreen
    {
        private readonly Camera _camera;

        public ActionCameraShaker()
        {
            _camera = Camera.main;
        }
        
        public void Action()
        {
            _camera.DOShakePosition(0.5f, 0.3f, 15, 90.0f, false);
        }

        public ActionType ActionType => ActionType.CAMERA_SHAKER;
    }
}