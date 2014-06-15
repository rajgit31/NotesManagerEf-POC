using System.Collections.Generic;
using NotesDomain;
using NotesDomain.Entities;
using NotesDomainInterfaces;

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

        public int Save(Note noteToSave)
        {
            var id = _noteRepo.Add(noteToSave);
            _unitOfWork.Save();
            return id;
        }

        public void Update(Note noteToUpdate)
        {
            _noteRepo.Update(noteToUpdate);
            _unitOfWork.Save();
        }

        public void Delete(Note noteDomain, bool disableSoftDelete = false)
        {
            if (disableSoftDelete)
            {
                _noteRepo.Delete(noteDomain);
            }
            else
            {
                noteDomain.MarkAsDeleted = true;
                _noteRepo.Update(noteDomain);
            }
            _unitOfWork.Save();
        } 

        public IEnumerable<Note> GetNotes()
        {
            return _noteRepo.All();
        }
    }
}
