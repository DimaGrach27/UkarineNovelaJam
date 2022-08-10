using ReflectionOfAmber.Scripts.GameModelBlock;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    public class CheckListSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private StatusEnum[] statusList;
        public override bool Check()
        {
            bool isCheck = true;

            foreach (var status in statusList)
            {
                isCheck &= SaveService.GetStatusValue(status);
            }
            
            return isCheck;
        }
    }
}