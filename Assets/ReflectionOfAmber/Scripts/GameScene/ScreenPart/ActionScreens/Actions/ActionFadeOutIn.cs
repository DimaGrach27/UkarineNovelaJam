using System.Collections;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFadeOutIn : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CoroutineHelper.StartCoroutine(FadeDaleOut());
        }

        private IEnumerator FadeDaleOut()
        {
            ActionScreenService.AudioSystemService.StopAllMusic();
            FadeService.FadeService.FadeIn(3.0f);
            
            yield return new WaitForSeconds(4.0f);
            
            FadeService.FadeService.FadeOut(3.0f);
        }

        public override ActionType ActionType => ActionType.FADE_OUT_IN;
    }
}