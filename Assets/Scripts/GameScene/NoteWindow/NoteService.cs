using System;
using UnityEngine;

namespace GameScene.NoteWindow
{
    public class NoteService
    {
        public event Action<int> OnChoose;

        public NoteService(Transform ui)
        {
            var noteWindowUIView = ui.GetComponentInChildren<NoteWindowUIView>();
            noteWindowUIView.OnChoose += OnChooseClick;
        }

        private void OnChooseClick(int index)
        {
            OnChoose?.Invoke(index);
        }
    }
}