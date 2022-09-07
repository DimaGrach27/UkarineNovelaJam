using ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc;
using UnityEngine;

namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Views
{
    public abstract class NoteWindowScreenBase : MonoBehaviour, INoteWindowScreen
    {
        public abstract NoteWindowScreensEnum NoteWindowScreensEnum { get; }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}