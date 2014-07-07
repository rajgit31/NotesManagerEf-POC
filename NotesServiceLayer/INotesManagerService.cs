using System.Collections.Generic;
using NotesDomain;
using NotesDomain.Entities;
using NotesManagerTransferEntities;

namespace NotesServiceLayer
{
    public interface INotesManagerService
    {
        IEnumerable<NoteDTO> GetNotes();
        NoteDTO FindById(int id);
        NoteDTO FindByTitle(string title);
        void Save(NoteDTO noteToSave);
        void SaveFiller(FillerFormDTO fillerFormDTO);
        void EditFiller(FillerFormDTO fillerFormDTO);
        int Update(NoteDTO noteToUpdate);
        int Delete(NoteDTO noteDomain, bool disableSoftDelete = false);
    }
}