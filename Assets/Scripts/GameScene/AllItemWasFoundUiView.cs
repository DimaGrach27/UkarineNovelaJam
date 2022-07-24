using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace GameScene
{
    public class AllItemWasFoundUiView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;

        private Tween _tween;
        private Coroutine _coroutine;

        public void Show()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(ShowItem());
        }

        private IEnumerator ShowItem()
        {
            if (_tween != null) DOTween.Kill(_tween);
            _tween = canvasGroup.DOFade(1.0f, 0.5f);
            yield return new WaitForSeconds(1.5f);
            _tween = canvasGroup.DOFade(0.0f, 0.5f);
        }
    }
}