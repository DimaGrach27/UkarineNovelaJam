using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Info SO", 
        menuName = "Frog Croaked Team/Special/Create 'Info SO'", 
        order = 0)]
    
    public class InfoSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private TranslatorKeys[] texts;
        [SerializeField] private InfoDescription infoDescription;

        public override bool Check()
        {
            InfoDescription infoDesc = Instantiate(infoDescription);
            infoDesc.SetInfoDescription(texts);
            infoDesc.ScreenPartsServiceFacade = ServiceFacade;
            
            return true;
        }
    }
}