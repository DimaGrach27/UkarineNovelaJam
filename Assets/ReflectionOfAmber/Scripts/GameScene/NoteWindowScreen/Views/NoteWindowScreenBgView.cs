using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public class NoteWindowScreenBgView : MonoBehaviour
    {
        [SerializeField] private Image bgScreen;

        [SerializeField] private Sprite firstPage;
        [SerializeField] private Sprite middlePage;

        public bool FirstPage
        {
            set => bgScreen.sprite = value ? firstPage : middlePage;
        }
        
        public void SetLastElement() => transform.SetAsLastSibling();
    }
}