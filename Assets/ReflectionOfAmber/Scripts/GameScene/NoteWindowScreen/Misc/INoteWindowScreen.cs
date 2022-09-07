namespace ReflectionOfAmber.Scripts.GameScene.NoteWindowScreen.Misc
{
    public interface INoteWindowScreen
    {
        public void Open();
        public void Close();
        public NoteWindowScreensEnum NoteWindowScreensEnum { get; }
    }
}