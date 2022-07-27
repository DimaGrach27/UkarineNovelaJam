using GameScene.Services;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionCrashSound : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.StarPlayMusic(MusicType.PREPARE_CRASH);
            AudioSystemService.Inst.AddQueueClip(MusicType.CRASH);
            AudioSystemService.Inst.AddQueueClip(MusicType.EMBIENT_SLOW);
        }

        public ActionType ActionType => ActionType.PLAY_CRASH_SOUND;
    }
}