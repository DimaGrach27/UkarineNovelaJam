using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFallenTree : IActionScreen
    {
        private readonly ActionScreenService _actionScreenService;
        public ActionFallenTree(ActionScreenService actionScreenService)
        {
            _actionScreenService = actionScreenService;
        }

        
        public void Action()
        {
            CoroutineHelper.Inst.StartCoroutine(CrashAnimaSound());
        }

        private IEnumerator CrashAnimaSound()
        {
            AudioSystemService.Inst.PlayShotSound(MusicType.FALLEN_TREE);
            yield return new WaitForSeconds(5.0f);
            _actionScreenService.Action(ActionType.CAMERA_SHAKER_LONG);
        }

        public ActionType ActionType => ActionType.FALLING_TREE;
    }
}