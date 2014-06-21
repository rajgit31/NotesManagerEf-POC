using System.Collections.Generic;
using System.Linq;
using NotesDomain;
using NotesDomain.Entities;
using NotesDomainInterfaces;
using NotesManagerTransferEntities;
using Omu.ValueInjecter;

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

        public NoteDTO FindById(int id)
        {
            var domain = _noteRepo.Find(new []{id});

            var dto = new NoteDTO();
            dto.InjectFrom(domain);
            return dto;
        }

        public NoteDTO FindByTitle(string title)
        {
            var domain = _noteRepo.Find(x => x.Title == title);
            var dto = new NoteDTO();
            dto.InjectFrom(domain);
            return dto;
        }

        public void Save(NoteDTO noteToSaveDTO)
        {
            //var noteToSave = noteToSaveDTO.ConvertToDomain();
            var noteEntity = new Note();
            noteEntity.InjectFrom<LoopValueInjection>(noteToSaveDTO);
            noteEntity.InjectFrom<MapEnum>(new { EntityState = noteToSaveDTO.EntityStateDTO });
            _noteRepo.Add(noteEntity);
            _unitOfWork.Save();
        }

        public int Update(NoteDTO noteToUpdate)
        {
            var noteEntity = new Note();
            noteEntity.InjectFrom<LoopValueInjection>(noteToUpdate);
            noteEntity.InjectFrom<MapEnum>(new { EntityState = noteToUpdate.EntityStateDTO });
            _noteRepo.Update(noteEntity);
            return _unitOfWork.Save();
        }

        public int Delete(NoteDTO noteDomain, bool disableSoftDelete = false)
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
            return _unitOfWork.Save();
        }

        public IEnumerable<NoteDTO> GetNotes()
        {
            var notes = _noteRepo.All();
            return notes.Select(x => x.ConvertToDTO());
        }
    }
}
