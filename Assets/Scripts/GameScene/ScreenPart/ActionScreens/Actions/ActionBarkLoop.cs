using GameScene.Services;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionBarkLoop  : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.LoopForLopper = true;
            AudioSystemService.Inst.StarPlayMusicOnLooper(MusicType.DOG_BARK);
        }

        public ActionType ActionType => ActionType.PLAY_DOG_BARK_SOUND_ON_LOPPER;
    }
    
    public class ActionBeepLoop  : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.LoopForLopper = true;
            AudioSystemService.Inst.StarPlayMusicOnLooper(MusicType.PHONE_BEEP);
        }

        public ActionType ActionType => ActionType.PLAY_BEEP_SOUND_ON_LOPPER;
    }
}