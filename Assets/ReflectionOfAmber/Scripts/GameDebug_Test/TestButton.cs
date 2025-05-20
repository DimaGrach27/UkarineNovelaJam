using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameDebug_Test
{
    public class TestButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        private Tween _tween;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            Debug.Log($"OnClick {gameObject.name}");
            EventSystem.current.SetSelectedGameObject(null);

            CallTween();
        }

        private void CallTween()
        {
            if (_tween != null)
            {
                _tween.Kill();
            }

            _tween = image.DOFade(0, 3.0f).OnComplete(() => { print("Tween Complete"); });
        }
    }
}