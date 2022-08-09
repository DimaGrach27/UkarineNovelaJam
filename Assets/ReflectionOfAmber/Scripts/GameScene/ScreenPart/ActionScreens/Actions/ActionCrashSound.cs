using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCrashSound : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CoroutineHelper.StartCoroutine(CrashAnimaSound());
        }

        private IEnumerator CrashAnimaSound()
        {
            ActionScreenService.AudioSystemService.PlayShotSound(MusicType.PREPARE_CRASH);
            AudioClip audioClip = ActionScreenService.AudioSystemService.GetClip(MusicType.PREPARE_CRASH);
            float prepareDuration = audioClip.length;
            yield return new WaitForSeconds(prepareDuration);
            ActionScreenService.Action(ActionType.CAMERA_SHAKER_LONG);
            ActionScreenService.AudioSystemService.PlayShotSound(MusicType.CRASH);
        }

        public override ActionType ActionType => ActionType.PLAY_CRASH_SOUND;
    }
}