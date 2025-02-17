using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GlobalProject;
using ReflectionOfAmber.Scripts.GlobalProject.Translator;
using ReflectionOfAmber.Scripts.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesView : UIScreenBase
    {
        [SerializeField] private ChapterNotesPartView chapterPartPrefab;
        [SerializeField] private ScrollRect container;
        [SerializeField] private Button closeButton;

        private int _countParts;

        public event Action OnCloseButtonClick;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnClickButtonCloseHandler);
        }

        public void Open(List<NoteChapterPart> chapters)
        {
            Open();
            
            while (_countParts < chapters.Count)
            {
                ChapterNotesPartView chapterNotesPart = Instantiate(chapterPartPrefab, container.content);
                chapterNotesPart.Name = TranslatorParser.GetText(chapters[_countParts].name);
                chapterNotesPart.Dialog = TranslatorParser.GetText(chapters[_countParts].text);
                _countParts++;
            }

            StartCoroutine(DelayOpen());
        }

        private IEnumerator DelayOpen()
        {
            yield return null;
            yield return null;
            yield return null;
            container.verticalScrollbar.value = 0.0f;
        }

        private void OnClickButtonCloseHandler()
        {
            OnCloseButtonClick?.Invoke();
        }
    }
}