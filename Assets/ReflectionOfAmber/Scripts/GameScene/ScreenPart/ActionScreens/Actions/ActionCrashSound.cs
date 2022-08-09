using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCrashSound : IActionScreen
    {
        private readonly ActionScreenService _actionScreenService;
        public ActionCrashSound(ActionScreenService actionScreenService)
        {
            _actionScreenService = actionScreenService;
        }
        
        public void Action()
        {
            CoroutineHelper.Inst.StartCoroutine(CrashAnimaSound());
        }

        private IEnumerator CrashAnimaSound()
        {
            AudioSystemService.Inst.PlayShotSound(MusicType.PREPARE_CRASH);
            AudioClip audioClip = AudioSystemService.Inst.GetClip(MusicType.PREPARE_CRASH);
            float prepareDuration = audioClip.length;
            yield return new WaitForSeconds(prepareDuration);
            _actionScreenService.Action(ActionType.CAMERA_SHAKER_LONG);
            AudioSystemService.Inst.PlayShotSound(MusicType.CRASH);
        }

        public ActionType ActionType => ActionType.PLAY_CRASH_SOUND;
    }
}