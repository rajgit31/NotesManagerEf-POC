using System.Collections.Generic;
using System.Linq;
using NotesDomain;
using NotesDomain.Entities;
using NotesDomainInterfaces;
using NotesManagerTransferEntities;

namespace NotesServiceLayer
{
    public class NotesManagerService : INotesManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Note> _noteRepo;

        public NotesManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _noteRepo = _unitOfWork.Repository<Note>();
        }

        public int Save(NoteDTO noteToSaveDTO)
        {
            var noteToSave = noteToSaveDTO.ConvertToDomain();
            var id = _noteRepo.Add(noteToSave);
            _unitOfWork.Save();
            return id;
        }

        public void Update(NoteDTO noteToUpdate)
        {
            var noteToSave = noteToUpdate.ConvertToDomain();
            _noteRepo.Update(noteToSave);
            _unitOfWork.Save();
        }

        public void Delete(NoteDTO noteDomain, bool disableSoftDelete = false)
        {
            var noteToSave = noteDomain.ConvertToDomain();

            if (disableSoftDelete)
            {
                _noteRepo.Delete(noteToSave);
            }
            else
            {
                noteDomain.MarkAsDeleted = true;
                _noteRepo.Update(noteToSave);
            }
            _unitOfWork.Save();
        }

        public IEnumerable<NoteDTO> GetNotes()
        {
            var notes = _noteRepo.All();
            return notes.Select(x => x.ConvertToDTO());
        }
    }
}
