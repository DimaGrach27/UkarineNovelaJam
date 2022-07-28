namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAddEvidenceForIlona : IActionScreen
    {
        public void Action()
        {
            GameModel.SetInt(KillerName.ILONA_VOR, 1);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_ILONA;
    }
    
    public class ActionAddEvidenceForZahares : IActionScreen
    {
        public void Action()
        {
            GameModel.SetInt(KillerName.ZAHARES_VOR, 1);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_ZAHARES;
    }
    
    public class ActionAddEvidenceForOleksii : IActionScreen
    {
        public void Action()
        {
            GameModel.SetInt(KillerName.OLEKSIY_VOR, 1);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_OLEKSII;
    }
}