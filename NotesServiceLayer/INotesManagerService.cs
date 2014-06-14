using System.Collections.Generic;
using NotesDomain;

namespace NotesServiceLayer
{
    public interface INotesManagerService
    {
        IEnumerable<Note> GetNotes();
        int Save(Note noteToSave);
        void Update(Note noteToUpdate);
        void Delete(Note noteDomain, bool disableSoftDelete = false);
    }
}