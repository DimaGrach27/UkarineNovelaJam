using System.Collections.Generic;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views;
using UnityEngine;
using UnityEngine.UI;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Handlers
{
    public class NoteWindowScreenChangeHandler : MonoBehaviour
    {
        [SerializeField] private NoteWindowScreenContainer[] noteWindows;
        [SerializeField] private Image bgScreen;

        [SerializeField] private Sprite firstPage;
        [SerializeField] private Sprite middlePage;

        private Dictionary<NoteWindowScreensEnum, NoteWindowScreenButton> _buttonsNoteMap;

        private void Awake()
        {
            _buttonsNoteMap = new();
            foreach (var button in GetComponentsInChildren<NoteWindowScreenButton>())
            {
                _buttonsNoteMap.Add(button.NoteWindowScreensEnum, button);
            }
        }

        private void Start()
        {
            foreach (var noteWindowScreenButton in _buttonsNoteMap.Values)
            {
                noteWindowScreenButton.OnClickButton += OnSelectWindowHandler;
            }

            OnSelectWindowHandler(NoteWindowScreensEnum.MAIN_SCREEN);
        }

        private void OnSelectWindowHandler(NoteWindowScreensEnum noteWindowScreensEnum)
        {
            bool isFirstPage = noteWindowScreensEnum == NoteWindowScreensEnum.MAIN_SCREEN;
            bgScreen.sprite = isFirstPage ? firstPage : middlePage;
            
            bgScreen.transform.SetAsLastSibling();
            _buttonsNoteMap[noteWindowScreensEnum].transform.SetAsLastSibling();

            foreach (var noteWindowScreen in noteWindows)
            {
                noteWindowScreen.GameObject.SetActive(noteWindowScreen.NoteWindowScreensEnum ==  noteWindowScreensEnum);
            }
        }

        private void OnDestroy()
        {
            foreach (var noteWindowScreenButton in _buttonsNoteMap.Values)
            {
                noteWindowScreenButton.OnClickButton -= OnSelectWindowHandler;
            }

            _buttonsNoteMap = null;
        }
    }
}