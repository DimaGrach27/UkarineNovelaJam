using GameScene.Services;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionRadioChange : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.StarPlayMusicOnLoop(MusicType.RADIO_CHANGE);
            AudioSystemService.Inst.AddQueueClipToLoop(MusicType.EMBIENT_SLOW);
        }

        public ActionType ActionType => ActionType.RADIO_CHANGE;
    }
}