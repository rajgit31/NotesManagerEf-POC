using System.Collections.Generic;
using NotesDomain;
using NotesDomain.Entities;
using NotesDomainInterfaces;
using NotesManagerTransferEntities;
using Omu.ValueInjecter;

namespace NotesServiceLayer
{
    public class StudentManagerService : IStudentManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Student> _studentRepo;
        private readonly IRepository<FriendStudentMapping> _mappingRepo;

        public StudentManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepo = _unitOfWork.Repository<Student>();
            _mappingRepo = _unitOfWork.Repository<FriendStudentMapping>();
        }

        public StudentDTO FindById(int id)
        {
            var domain = _studentRepo.Find(new[] { id });

            var dto = new StudentDTO();
            dto.InjectFrom(domain);
            return dto;
        }

        public StudentDTO FindByTitle(string title)
        {
            var domain = _studentRepo.Find(x => x.Name == title);
            var dto = new StudentDTO();
            dto.InjectFrom(domain);
            return dto;
        }

        public void Save(StudentDTO noteToSaveDTO)
        {
            //var noteToSave = noteToSaveDTO.ConvertToDomain();
            
            
            var studentEntity = new Student();
            studentEntity.EntityState = EntityState.Added;
            studentEntity.InjectFrom<LoopValueInjection>(noteToSaveDTO);
            studentEntity.InjectFrom<MapEnum>(new { EntityState = studentEntity.EntityState });
            _studentRepo.Add(studentEntity);
            _unitOfWork.Save();
        }

        public void Save(FriendStudentMappingDTO noteToSave)
        {
            var mappingEntity = new FriendStudentMapping();
            mappingEntity.EntityState = EntityState.Added;
            mappingEntity.Student = new Student
            {
                Id = noteToSave.StudentDTO.Id,
                Name = noteToSave.StudentDTO.Name,
                EntityState = EntityState.Unchanged,
            };

            _mappingRepo.Add(mappingEntity);
            _unitOfWork.Save();
        }

        public int Update(StudentDTO noteToUpdate)
        {
            throw new System.NotImplementedException();
        }

      

        public int Delete(StudentDTO noteDomain, bool disableSoftDelete = false)
        {
            //var noteToSave = noteDomain.ConvertToDomain();

            //if (disableSoftDelete)
            //{
            //    _studentRepo.Delete(noteToSave);
            //}
            //else
            //{
            //    noteDomain.MarkAsDeleted = true;
            //    _studentRepo.Update(noteToSave);
            //}
            //return _unitOfWork.Save();
            return 1;
        }

        public IEnumerable<StudentDTO> GetNotes()
        {
            var notes = _studentRepo.All();
            return null;
        }
    }
}