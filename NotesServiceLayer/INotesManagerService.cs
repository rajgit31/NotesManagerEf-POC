using System.Collections.Generic;
using NotesDomain;
using NotesDomain.Entities;
using NotesManagerTransferEntities;

namespace NotesServiceLayer
{
    public interface INotesManagerService
    {
        IEnumerable<NoteDTO> GetNotes();
        int Save(NoteDTO noteToSave);
        void Update(NoteDTO noteToUpdate);
        void Delete(NoteDTO noteDomain, bool disableSoftDelete = false);
    }
}