using System;
using System.Collections.Generic;
using System.Linq;
using NotesDataAccesLayer;
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
        private readonly IRepository<FillerForm> _fillerFormRepo;
        private readonly IRepository<QuestionAnswerMapping> _questionAnswerMapRepo;

        public NotesManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _noteRepo = _unitOfWork.Repository<Note>();
            _fillerFormRepo = _unitOfWork.Repository<FillerForm>();
            _questionAnswerMapRepo = _unitOfWork.Repository<QuestionAnswerMapping>();
        }

        public NoteDTO FindById(int id)
        {
            var domain = _noteRepo.Find(id);

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
            noteEntity.EntityState = EntityState.Added;
            _noteRepo.Add(noteEntity);
            _unitOfWork.Save();
        }

        public void SaveFiller(FillerFormDTO fillerFormDTO)
        {
            _unitOfWork.BeginTransaction();

            try
            {
                var fillerFormEntity = new FillerForm()
                {
                    EntityState = EntityState.Added,
                    Name = fillerFormDTO.Name,
                    QuestionAnswerMappings = GetMappings(fillerFormDTO.QuestionAnswerMappings).ToList()
                };
                _fillerFormRepo.Add(fillerFormEntity);
                _unitOfWork.Save();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
        }

        public void EditFiller(FillerFormDTO fillerFormDTO)
        {
            fillerFormDTO.QuestionAnswerMappings.ForEach(x => x.EntityStateDTO = EntityStateDTO.Modified);

            _unitOfWork.BeginTransaction();

            try
            {
                var fillerForm = _fillerFormRepo.GetFillerMap(3);
                foreach (var emap in fillerForm.QuestionAnswerMappings)
                {
                    emap.Client = "ClientEdit";
                    _questionAnswerMapRepo.Update(emap);
                }

                _unitOfWork.Save();
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
            }
        }

        private IList<QuestionAnswerMapping> GetMappingsEdit(IEnumerable<QuestionAnswerMappingDTO> dtos, FillerForm fillerForm)
        {
            var list = new List<QuestionAnswerMapping>();

            foreach (var questionAnswerMappingDto in dtos)
            {
                list.Add(new QuestionAnswerMapping()
                {
                    Id = 3,
                    Client = questionAnswerMappingDto.Client,
                    EntityState = EntityState.Modified,
                    Note = GetExistingNote(questionAnswerMappingDto.NoteDTO.Id),
                    FillerForm = fillerForm
                });
            }

            return list;
        }

        private IList<QuestionAnswerMapping> GetMappings(IEnumerable<QuestionAnswerMappingDTO> dtos )
        {
            var list = new List<QuestionAnswerMapping>();

            foreach (var questionAnswerMappingDto in dtos)
            {
                list.Add(new QuestionAnswerMapping()
                {
                    Client = questionAnswerMappingDto.Client,
                    EntityState = EntityState.Added,
                    Note = GetExistingNote(questionAnswerMappingDto.NoteDTO.Id)
                });
            }

            return list;
        }

        private IList<QuestionAnswerMapping> GetExistingMappings(IEnumerable<QuestionAnswerMappingDTO> dtos)
        {
            var list = new List<QuestionAnswerMapping>();

            foreach (var questionAnswerMappingDto in dtos)
            {
                _questionAnswerMapRepo.Find(questionAnswerMappingDto.Id);
            }

            return list;
        }

        private Note GetExistingNote(int id)
        {
            var find = _noteRepo.Find(id);
            return find;
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
