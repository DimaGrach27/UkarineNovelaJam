using ReflectionOfAmber.Scripts.GlobalProject;

namespace ReflectionOfAmber.Scripts.GameScene.ChapterNotes
{
    public class ChapterNotesSaveService
    {
        public void SetDialogPart(string name, string text)
        {
            ChapterNotesFile chapterNotesFile = SaveService.ChapterNotesFile;
            NoteChapterPart noteChapterPart = new NoteChapterPart
            {
                name = name,
                text = text
            };
            
            chapterNotesFile.chapters.Add(noteChapterPart);
            SaveService.SaveChapterNotesJson();
        }
    }
}