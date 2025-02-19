using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using UnityEngine;
using Zenject;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Info SO", 
        menuName = "Frog Croaked Team/Special/Create 'Info SO'", 
        order = 0)]
    
    public class InfoSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private TranslatorKeys[] texts;
        [SerializeField] private bool m_showCameraIcon;
        
        [Inject] 
        private InfoDescription.Factory m_infoDescriptionFactory;
        
        public override bool Check()
        {
            InfoDescription infoDesc = m_infoDescriptionFactory.Create();
            infoDesc.SetInfoDescription(texts, m_showCameraIcon);
            infoDesc.ScreenPartsServiceFacade = ServiceFacade;
            
            return true;
        }
    }
}