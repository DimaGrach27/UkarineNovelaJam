using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.ScreenPart.SpecialSO
{
    [CreateAssetMenu(
        fileName = "Titre SO", 
        menuName = "Frog Croaked Team/Special/Create 'Titre SO'", 
        order = 0)]
    public class TitreSpecialScriptableObject : SpecialScriptableObjectBase
    {
        [SerializeField] private GameObject titreAnima;
        
        public override bool Check()
        {
            GameObject titre = Instantiate(titreAnima);
            
            Destroy(titre, 31.0f);
            
            return false;
        }
    }
}