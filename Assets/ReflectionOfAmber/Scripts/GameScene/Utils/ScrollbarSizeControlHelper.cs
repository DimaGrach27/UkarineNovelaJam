using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.Utils
{
    public class ScrollbarSizeControlHelper : MonoBehaviour
    {
        [SerializeField, Range(0.0f, 1.0f)] private float size = 0.2f;

        private Scrollbar _scrollbar;

        private void Awake()
        {
            _scrollbar = GetComponent<Scrollbar>();
            _scrollbar.onValueChanged.AddListener(OnChangeValue);
            OnChangeValue();
        }

        private void OnChangeValue(float value = 0.0f)
        {
            _scrollbar.size = size;
        }
    }
}