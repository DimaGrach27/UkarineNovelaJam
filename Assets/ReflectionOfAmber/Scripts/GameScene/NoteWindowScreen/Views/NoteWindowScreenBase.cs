using DG.Tweening;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class NoteWindowScreenBase : MonoBehaviour, INoteWindowScreen
    {
        private CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == null)
                    _canvasGroup = GetComponent<CanvasGroup>();
                return _canvasGroup;
            }
        }
        private CanvasGroup _canvasGroup;
        private Tween _fade;
        
        public abstract NoteWindowScreensEnum NoteWindowScreensEnum { get; }

        public abstract Selectable GetFirstSelectable();

        public virtual void Open()
        {
            gameObject.SetActive(true);
            FadeInWindow();
        }

        public virtual void Close()
        {
            if (_fade != null) DOTween.Kill(_fade);
            gameObject.SetActive(false);
        }
        
        private void FadeInWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            
            CanvasGroup.alpha = 0.0f;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(1.0f, duration);
        }
    }
}