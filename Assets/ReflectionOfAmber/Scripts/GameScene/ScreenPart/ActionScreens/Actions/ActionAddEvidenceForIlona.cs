using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAddEvidenceForIlona : ActionBase
    {
        public override void Action()
        {
            int count = SaveService.GetIntValue(KillerName.ILONA_VOR);
            count++;
            
            SaveService.SetIntValue(KillerName.ILONA_VOR, count);
        }

        public override ActionType ActionType => ActionType.ADD_EVIDENCE_ILONA;
    }
    
    public class ActionAddEvidenceForZahares : ActionBase
    {
        public override void Action()
        {
            int count = SaveService.GetIntValue(KillerName.ZAHARES_VOR);
            count++;
            
            SaveService.SetIntValue(KillerName.ZAHARES_VOR, count);
        }

        public override ActionType ActionType => ActionType.ADD_EVIDENCE_ZAHARES;
    }
    
    public class ActionAddEvidenceForOleksii : ActionBase
    {
        public override void Action()
        {
            int count = SaveService.GetIntValue(KillerName.OLEKSIY_VOR);
            count++;
            
            SaveService.SetIntValue(KillerName.OLEKSIY_VOR, count);
        }

        public override ActionType ActionType => ActionType.ADD_EVIDENCE_OLEKSII;
    }
}