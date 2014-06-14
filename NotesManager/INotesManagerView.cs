using NotesManager.ViewModels;

namespace NotesManager
{
    public interface INotesManagerView
    {
        NoteViewModel NoteToAdd { get; }
        NoteViewModel NoteToEdit { get; }
        NoteViewModel NoteToDelete { get; }
    }
}