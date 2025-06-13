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
        [SerializeField] private ButtonExt closeButton;

        private int m_countParts;
        private bool m_isOpened = false;

        public event Action OnCloseButtonClick;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnClickButtonCloseHandler);
        }

        private void Update()
        {
            if (!m_isOpened)
            {
                return;
            }
            
            container.verticalScrollbar.ScrollByMouseWheel();
        }

        public void Open(List<NoteChapterPart> chapters)
        {
            Open();
            
            while (m_countParts < chapters.Count)
            {
                ChapterNotesPartView chapterNotesPart = Instantiate(chapterPartPrefab, container.content);
                chapterNotesPart.Name = TranslatorService.GetText(chapters[m_countParts].name);
                chapterNotesPart.Dialog = TranslatorService.GetText(chapters[m_countParts].text);
                m_countParts++;
            }

            StartCoroutine(DelayOpen());
            
            FocusUIManager.Instance.JumpSelectionToObject(closeButton);
        }

        private IEnumerator DelayOpen()
        {
            yield return null;
            yield return null;
            yield return null;
            container.verticalScrollbar.value = 0.0f;

            m_isOpened = true;
        }

        private void OnClickButtonCloseHandler()
        {
            m_isOpened = false;
            OnCloseButtonClick?.Invoke();
        }
    }
}