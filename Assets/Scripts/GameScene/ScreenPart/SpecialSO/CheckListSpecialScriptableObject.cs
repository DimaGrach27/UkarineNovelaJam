using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    public class CheckListSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private NextScene nextScene;
        [SerializeField] private StatusEnum[] statusList;
        public override bool Check()
        {
            bool isCheck = true;

            foreach (var status in statusList)
            {
                isCheck &= GameModel.GetStatus(status);
            }
            
            return isCheck;
        }

        public override NextScene NextScene()
        {
            return nextScene;
        }
    }
}