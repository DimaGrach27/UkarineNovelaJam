using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Info SO", 
        menuName = "Frog Croaked Team/Special/Create 'Info SO'", 
        order = 0)]
    
    public class InfoSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private InfoDescription infoDescription;
        
        public override bool Check()
        {
            Object.Instantiate(infoDescription.gameObject);
            return true;
        }
    }
}