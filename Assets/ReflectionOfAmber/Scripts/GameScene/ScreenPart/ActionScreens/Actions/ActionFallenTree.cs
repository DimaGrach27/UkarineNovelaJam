using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFallenTree : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CoroutineHelper.StartCoroutine(CrashAnimaSound());
        }

        private IEnumerator CrashAnimaSound()
        {
            ActionScreenService.AudioSystemService.PlayShotSound(MusicType.FALLEN_TREE);
            yield return new WaitForSeconds(5.0f);
            ActionScreenService.Action(ActionType.CAMERA_SHAKER_LONG);
        }

        public override ActionType ActionType => ActionType.FALLING_TREE;
    }
}