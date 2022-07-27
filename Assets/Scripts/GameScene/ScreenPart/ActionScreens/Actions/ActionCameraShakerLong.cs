using DG.Tweening;
using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCameraShakerLong  : IActionScreen
    {
        private readonly Camera _camera;

        public ActionCameraShakerLong()
        {
            _camera = Camera.main;
        }
        
        public void Action()
        {
            _camera.DOShakePosition(1.25f, 0.35f, 15, 90.0f, false);
        }

        public ActionType ActionType => ActionType.CAMERA_SHAKER_LONG;
    }
}