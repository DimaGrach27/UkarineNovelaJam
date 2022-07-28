using GameScene.Services;

namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionWaterSlap : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.PlayShotSound(MusicType.WATER_SLAP);
        }

        public ActionType ActionType => ActionType.WATER_SLAP_SOUND;
    }
    
    public class ActionDragMetal : IActionScreen
    {
        public void Action()
        {
            AudioSystemService.Inst.PlayShotSound(MusicType.METAL_DRAG);
        }

        public ActionType ActionType => ActionType.METAL_DRAG_SOUND;
    }
}