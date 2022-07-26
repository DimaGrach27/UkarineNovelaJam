using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    public class CheckListSpecialScriptableObject : SpecialScriptableObjectBase
    {
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
    }
}