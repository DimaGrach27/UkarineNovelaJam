using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Characters
{
    public class CharacterUiView : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private CharacterScreenPositionEnum screenPosition;

        public bool Visible
        {
            set => gameObject.SetActive(value);
        }

        public Sprite Image
        {
            set
            {
                if (value == null)
                {
                    Visible = false;
                    return;
                }
                
                if(value == image.sprite)
                {
                    Visible = true;
                    return;
                }

                Color color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                
                image.color = color;
                image.sprite = value;
                image.SetNativeSize();
                
                Dissolve();
                
                Visible = true;
            }
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        private void Dissolve(float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION)
        {
            image.DOFade(1.0f, duration);
        }

        public CharacterScreenPositionEnum ScreenPosition => screenPosition;
    }
}