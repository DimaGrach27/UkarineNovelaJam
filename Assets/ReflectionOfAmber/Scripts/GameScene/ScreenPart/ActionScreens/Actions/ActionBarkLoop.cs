using System.Collections;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.Services;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionBarkLoop : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.LoopForLopper = true;
            ActionScreenService.AudioSystemService.StarPlayMusicOnLooper(MusicType.DOG_BARK);
        }

        public override ActionType ActionType => ActionType.PLAY_DOG_BARK_SOUND_ON_LOPPER;
    }
    
    public class ActionBeepLoop : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.LoopForLopper = true;
            ActionScreenService.AudioSystemService.StarPlayMusicOnLooper(MusicType.PHONE_BEEP);
        }

        public override ActionType ActionType => ActionType.PLAY_BEEP_SOUND_ON_LOPPER;
    }
    
    public class ActionHeartBeepLoop : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.LoopForLopper = true;
            ActionScreenService.AudioSystemService.StarPlayMusicOnLooper(MusicType.HEART_BEEP);
        }

        public override ActionType ActionType => ActionType.PLAY_HEART_BEEP_SOUND_ON_LOPPER;
    }
    
    public class ActionEmbientSlow : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.CoroutineHelper.StartCoroutine(ChangeMusicRoutine());
        }

        private IEnumerator ChangeMusicRoutine()
        {
            ActionScreenService.AudioSystemService.AudioSourceMusic.DOFade(0.0f, 1.0f);
            yield return new WaitForSeconds(1.0f);
            ActionScreenService.AudioSystemService.ChangeMusic(SaveService.GetMusicVolume());
            ActionScreenService.AudioSystemService.StarPlayMusicOnLoop(MusicType.EMBIENT_SLOW);
        }

        public override ActionType ActionType => ActionType.PLAY_EMBIENT_SLOW;
    }
    
    public class ActionEmbientFast : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.StarPlayMusicOnLoop(MusicType.EMBIENT_FAST);
        }

        public override ActionType ActionType => ActionType.PLAY_EMBIENT_FAST;
    }
}