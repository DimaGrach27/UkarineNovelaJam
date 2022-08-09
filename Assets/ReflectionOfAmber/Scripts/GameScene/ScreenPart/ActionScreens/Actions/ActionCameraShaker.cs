using DG.Tweening;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCameraShaker : ActionBase
    {
        private Camera _camera;
        private Camera Camera
        {
            get
            {
                if(_camera == null)
                    _camera = Camera.main;
                
                return _camera;
            }
        }

        public override void Action()
        {
            Camera.DOShakePosition(0.25f, 0.3f, 15, 90.0f, false);
        }

        public override ActionType ActionType => ActionType.CAMERA_SHAKER;
    }
}