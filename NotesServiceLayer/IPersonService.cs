using NotesManagerTransferEntities;

namespace NotesServiceLayer
{
    public interface IPersonService
    {
        void Save(PersonDTO noteToSaveDTO);
        void Update(PersonDTO noteToSaveDTO);
    }
}