using System.Collections;
using DG.Tweening;
using GameScene.Services;
using UnityEngine;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFadeOutMusicOnLooper : IActionScreen
    {
        public void Action()
        {
            CoroutineHelper.Inst.StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            float duration = 1.5f;

            AudioSystemService.Inst.AudioSourceMusic.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            AudioSystemService.Inst.ChangeAudio(SaveService.GetMusicVolume());
            AudioSystemService.Inst.StopSoundMusic();
        }
        
        public ActionType ActionType => ActionType.FADE_OUT_MUSIC_ON_LOPPER;
    }
    
    public class ActionFadeInMusicOnLooper : IActionScreen
    {
        public void Action()
        {
            float duration = 1.5f;
            AudioSystemService.Inst.AudioSourceMusic.volume = 0.0f;
            AudioSystemService.Inst.AudioSourceMusic.DOFade(SaveService.GetMusicVolume(), duration);
        }

        public ActionType ActionType => ActionType.FADE_IN_MUSIC_ON_LOPPER;
    }
    
    public class ActionStopMusicOnLooper : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.StopSoundMusic();
        }

        public ActionType ActionType => ActionType.STOP_MUSIC_ON_LOPPER;
    }
}