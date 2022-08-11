using ReflectionOfAmber.Scripts.GameScene.TitreScreen;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Titre SO", 
        menuName = "Frog Croaked Team/Special/Create 'Titre SO'", 
        order = 0)]
    public class TitreSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private TitreAnimation titreAnima;
        
        public override bool Check()
        {
            TitreAnimation titre = Instantiate(titreAnima);
            titre.OnEndAnimation += OnEndAnimationHandler;
            
            return false;
        }

        private void OnEndAnimationHandler()
        {
            ServiceFacade.PlatNextPart();
        }
    }
}