using System;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc
{
    [Serializable]
    public class NoteWindowScreenContainer
    {
        [SerializeField] private NoteWindowScreensEnum noteWindowScreensEnum;
        [SerializeField] private GameObject gameObject;

        public NoteWindowScreensEnum NoteWindowScreensEnum => noteWindowScreensEnum;
        public GameObject GameObject => gameObject;
    }
}