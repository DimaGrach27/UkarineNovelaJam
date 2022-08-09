using ReflectionOfAmber.Scripts.GameScene.Services;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionWaterSlap : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.PlayShotSound(MusicType.WATER_SLAP);
        }

        public override ActionType ActionType => ActionType.WATER_SLAP_SOUND;
    }
    
    public class ActionDragMetal : ActionBase
    {
        public override void Action()
        {
            ActionScreenService.AudioSystemService.PlayShotSound(MusicType.METAL_DRAG);
        }

        public override ActionType ActionType => ActionType.METAL_DRAG_SOUND;
    }
}