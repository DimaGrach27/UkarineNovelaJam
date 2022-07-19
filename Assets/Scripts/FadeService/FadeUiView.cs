using UnityEngine;

namespace FadeService
{
    public class FadeUiView : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }

        public float Fade
        {
            set => CanvasGroup.alpha = value;
        }

        public bool Visible
        {
            set => gameObject.SetActive(value);
        }
    }
}