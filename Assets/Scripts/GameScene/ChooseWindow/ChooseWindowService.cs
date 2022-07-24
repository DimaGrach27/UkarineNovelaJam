using System;
using System.Collections;
using DG.Tweening;
using GameScene.ScreenPart;
using UnityEngine;

namespace GameScene.ChooseWindow
{
    public class ChooseWindowService
    {
        private readonly ChooseWindowUiView _chooseWindowUiView;

        public event Action<NextScene> OnChoose;

        private Coroutine _coroutine;

        public ChooseWindowService(Transform transform)
        {
            _chooseWindowUiView = transform.GetComponentInChildren<ChooseWindowUiView>();
            _chooseWindowUiView.OnChoose += OnChooseClick;
        }

        public void SetChooses(NextScene[] nextScenes)
        {
            if(_coroutine !=null) CoroutineHelper.Inst.StopCoroutine(_coroutine);
            _coroutine = CoroutineHelper.Inst.StartCoroutine(FadeInWindow());
            
            _chooseWindowUiView.InitButtons(nextScenes);
        }

        public void ChangeVisible(bool isVisible) => _chooseWindowUiView.Visible = isVisible;
        
        private void OnChooseClick(NextScene chooseScene)
        {
            if(_coroutine !=null) CoroutineHelper.Inst.StopCoroutine(_coroutine);
            _coroutine = CoroutineHelper.Inst.StartCoroutine(FadeOutWindow());
            
            OnChoose?.Invoke(chooseScene);
        }

        private IEnumerator FadeInWindow()
        {
            _chooseWindowUiView.Visible = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _chooseWindowUiView.CanvasGroup.DOFade(1.0f, duration);
            yield return null;
        }
        
        private IEnumerator FadeOutWindow()
        {
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _chooseWindowUiView.CanvasGroup.DOFade(0.0f, duration);
            yield return new WaitForSeconds(duration);
            _chooseWindowUiView.Visible = false;
        }
    }
}