using System.Collections;
using ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens;
using ReflectionOfAmber.Scripts;
using ReflectionOfAmber.Scripts.GameScene.Services;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFadeOutIn : IActionScreen
    {
        public void Action()
        {
            CoroutineHelper.Inst.StartCoroutine(FadeDaleOut());
        }

        private IEnumerator FadeDaleOut()
        {
            AudioSystemService.Inst.StopAllMusic();
            FadeService.FadeService.FadeIn(3.0f);
            
            yield return new WaitForSeconds(4.0f);
            
            FadeService.FadeService.FadeOut(3.0f);
        }

        public ActionType ActionType => ActionType.FADE_OUT_IN;
    }
}