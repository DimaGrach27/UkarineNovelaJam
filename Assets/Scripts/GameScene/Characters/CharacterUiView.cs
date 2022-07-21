using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Characters
{
    public class CharacterUiView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private CharacterScreenPositionEnum screenPosition;
        public Image Image => image;

        public bool Visible
        {
            set => gameObject.SetActive(value);
        }
        
        public Sprite Sprite
        {
            set
            {
                image.sprite = value;
                image.SetNativeSize();
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public CharacterScreenPositionEnum ScreenPosition => screenPosition;
    }
}