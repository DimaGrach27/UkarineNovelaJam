using System.Collections.Generic;
using DG.Tweening;
using ReflectionOfAmber.Scripts.GlobalProject;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ChapterNotesView : MonoBehaviour
    {
        [SerializeField] private ChapterNotesPartView chapterPartPrefab;
        [SerializeField] private ScrollRect container;
        [SerializeField] private Button closeButton;

        private int _countParts;
        
        private CanvasGroup CanvasGroup => _canvasGroup ??= GetComponent<CanvasGroup>();
        private CanvasGroup _canvasGroup;

        private Tween _fade;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnClickButtonCloseHandler);
        }

        public void Open(List<NoteChapterPart> chapters)
        {
            FadeInWindow();
            GlobalEvent.HideCanvas();
            
            while (_countParts < chapters.Count)
            {
                ChapterNotesPartView chapterNotesPart = Instantiate(chapterPartPrefab, container.content);
                chapterNotesPart.Name = chapters[_countParts].name;
                chapterNotesPart.Dialog = chapters[_countParts].text;
                _countParts++;
            }

            container.verticalScrollbar.value = 0.0f;
        }

        private void OnClickButtonCloseHandler()
        {
            GlobalEvent.ShowCanvas();
            FadeOutWindow();
        }
        
        private void FadeInWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(1.0f, duration);
        }
        
        private void FadeOutWindow()
        {
            if (_fade != null) DOTween.Kill(_fade);
            
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;
            float duration = GlobalConstant.ANIMATION_DISSOLVE_DURATION;
            _fade = CanvasGroup.DOFade(0.0f, duration);
        }
    }
}