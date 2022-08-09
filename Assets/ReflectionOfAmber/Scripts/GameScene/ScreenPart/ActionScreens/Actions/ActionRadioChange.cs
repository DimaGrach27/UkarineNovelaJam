using ReflectionOfAmber.Scripts.GameScene.Services;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionRadioChange : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.StarPlayMusicOnLoop(MusicType.RADIO_CHANGE);
            ActionScreenService.AudioSystemService.AddQueueClipToLoop(MusicType.EMBIENT_SLOW);
        }

        public override ActionType ActionType => ActionType.RADIO_CHANGE;
    }
}