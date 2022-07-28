using System.Collections.Generic;
using UnityEngine;

namespace GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Info SO", 
        menuName = "Frog Croaked Team/Special/Create 'Info SO'", 
        order = 0)]
    
    public class InfoSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField, TextArea(1, 5)] private List<string> texts;
        [SerializeField] private InfoDescription infoDescription;
        
        public override bool Check()
        {
            InfoDescription infoDesc = Instantiate(infoDescription);
            infoDesc.SetInfoDescription(texts);
            
            return true;
        }
    }
}