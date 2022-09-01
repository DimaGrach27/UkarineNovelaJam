using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionFadeOutMusicOnLooper : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CoroutineHelper.StartCoroutine(Fade());
        }

        private IEnumerator Fade()
        {
            float duration = 1.5f;

            ActionScreenService.AudioSystemService.SoundAudioLooperSource.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            ActionScreenService.AudioSystemService.ChangeAudio(SaveService.MusicVolume);
            ActionScreenService.AudioSystemService.StopSoundMusic();
        }
        
        public override ActionType ActionType => ActionType.FADE_OUT_MUSIC_ON_LOPPER;
    }
    
    public class ActionFadeInMusicOnLooper : ActionBase
    {
        public override void Action()
        {
            float duration = 1.5f;
            ActionScreenService.AudioSystemService.SoundAudioLooperSource.volume = 0.0f;
            ActionScreenService.AudioSystemService.SoundAudioLooperSource.DOFade(SaveService.MusicVolume, duration);
        }

        public override ActionType ActionType => ActionType.FADE_IN_MUSIC_ON_LOPPER;
    }
    
    public class ActionStopMusicOnLooper : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.StopSoundMusic();
        }

        public override ActionType ActionType => ActionType.STOP_MUSIC_ON_LOPPER;
    }
    
    public class ActionStopAllMusic : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.StopAllMusic();
        }

        public override ActionType ActionType => ActionType.STOP_ALL_MUSIC;
    }
}