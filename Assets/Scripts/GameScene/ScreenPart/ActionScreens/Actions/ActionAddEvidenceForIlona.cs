namespace GameScene.ScreenPart.ActionScreens.Actions
{
    public class ActionAddEvidenceForIlona : IActionScreen
    {
        public void Action()
        {
            int count = GameModel.GetInt(KillerName.ILONA_VOR);
            count++;
            
            GameModel.SetInt(KillerName.ILONA_VOR, count);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_ILONA;
    }
    
    public class ActionAddEvidenceForZahares : IActionScreen
    {
        public void Action()
        {
            int count = GameModel.GetInt(KillerName.ZAHARES_VOR);
            count++;
            
            GameModel.SetInt(KillerName.ZAHARES_VOR, count);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_ZAHARES;
    }
    
    public class ActionAddEvidenceForOleksii : IActionScreen
    {
        public void Action()
        {
            int count = GameModel.GetInt(KillerName.OLEKSIY_VOR);
            count++;
            
            GameModel.SetInt(KillerName.OLEKSIY_VOR, count);
        }

        public ActionType ActionType => ActionType.ADD_EVIDENCE_OLEKSII;
    }
}