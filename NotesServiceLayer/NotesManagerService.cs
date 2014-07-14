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
        private readonly IRepository<AcpAnswer> _acpAnswerRepo;
        private readonly IRepository<AcpQuestionListItem> _acpQuesListItemRepo;

        public NotesManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _noteRepo = _unitOfWork.Repository<Note>();
            _fillerFormRepo = _unitOfWork.Repository<FillerForm>();
            _questionAnswerMapRepo = _unitOfWork.Repository<QuestionAnswerMapping>();
            _acpAnswerRepo = _unitOfWork.Repository<AcpAnswer>();
            _acpQuesListItemRepo = _unitOfWork.Repository<AcpQuestionListItem>();
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
                //
                var questionAnswerMapping = _questionAnswerMapRepo.Find(1); //this helps to rehidtrate the context with acpQAMap
                var acpQAnsMap = new QuestionAnswerMapping();
                acpQAnsMap.Id = 1; //existing id
                acpQAnsMap.Client = "Newclient7";
                acpQAnsMap.EntityState = EntityState.Modified;
                acpQAnsMap.FK_FillerFormId = questionAnswerMapping.FK_FillerFormId;
                
                _questionAnswerMapRepo.Update(acpQAnsMap);

                
                //
                var answerE = _acpAnswerRepo.Find(1); //this helps to rehidtrate the context with acpAnswer
                var questionListItemE = _acpQuesListItemRepo.Find(5); //the eixting we want to update - first hidrate it into context

                //var questionLstItm = new AcpQuestionListItem()
                //{
                //    Id = 5,
                //    EntityState = EntityState.Unchanged,
                //};

                //_acpQuesListItemRepo.Attach(questionListItemE);

                var answer = new AcpAnswer
                {
                    Id = 1,
                    EntityState = EntityState.Modified,
                    AnswerText = "answer7",
                    AcpQuestionListItem = questionListItemE
                };

                
                _acpAnswerRepo.Update(answer);
               
                

             

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

        private IList<QuestionAnswerMapping> GetMappings(IEnumerable<QuestionAnswerMappingDTO> dtos)
        {
            var list = new List<QuestionAnswerMapping>();

            foreach (var d in dtos)
            {
                list.Add(new QuestionAnswerMapping()
                {
                    Client = d.Client,
                    EntityState = EntityState.Added,
                    Note = GetExistingNote(d.NoteDTO.Id),
                    AcpAnswer = new AcpAnswer()
                    {
                        EntityState = EntityState.Added, AnswerText = d.AcpAnswerDTO.AnswerText,
                        AcpQuestionListItem = new AcpQuestionListItem
                        {
                            EntityState = EntityState.Unchanged,
                            Id = 1,
                        }
                    }
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

