using System.Collections.Generic;
using NotesManagerTransferEntities;

namespace NotesServiceLayer
{
    public interface IStudentManagerService
    {
        StudentDTO FindById(int id);
        StudentDTO FindByTitle(string title);
        void Save(StudentDTO noteToSaveDTO);
        void Save(FriendStudentMappingDTO noteToSave);
        int Update(StudentDTO noteToUpdate);
        int Delete(StudentDTO noteDomain, bool disableSoftDelete = false);
        IEnumerable<StudentDTO> GetNotes();
    }
}