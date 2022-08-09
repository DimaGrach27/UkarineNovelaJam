using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
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
            _camera.DOShakePosition(0.25f, 0.3f, 15, 90.0f, false);
        }

        public ActionType ActionType => ActionType.CAMERA_SHAKER;
    }
}