using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using NotesDomain.Entities;
using NotesDomainInterfaces;
using NotesManagerTransferEntities;
using Omu.ValueInjecter;

namespace NotesServiceLayer
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWorkSimple _unitOfWork;
        private readonly IRepositorySimple<Person> _personRepo;
        private readonly IRepositorySimple<Passport> _passportRepo;

        public PersonService(IUnitOfWorkSimple unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _personRepo = _unitOfWork.Repository<Person>();
            _passportRepo = _unitOfWork.Repository<Passport>();
        }

        public void Save(PersonDTO noteToSaveDTO)
        {
            var person = new Person();
            person.InjectFrom(noteToSaveDTO);
            person.Passport = new Passport();
            person.Passport.InjectFrom(noteToSaveDTO.PassportDTO);
            _personRepo.Add(person);

            _passportRepo.Add(new Passport(){ Number = "SCSCSCS", Nationality = "American"});

            _unitOfWork.Save();


        }

        public void Update(PersonDTO noteToSaveDTO)
        {
            //1. load existing entity and modify
            //var existingPerson = _personRepo.All().Single();
            //var americanpassport = _passportRepo.Find(3);
            //existingPerson.Name = "Bush";
            //existingPerson.Passport = americanpassport;
            //_personRepo.Update(existingPerson);

            //2. Eager load and attach the foreigh key
            //var existingPerson = _personRepo.TestEagerLoad();
            //existingPerson.Name = "Bush";
            //var passportEx = new Passport() {Id = 8}; //existing id
            //_passportRepo.Attach(passportEx);
            //existingPerson.Passport = passportEx;
            //_personRepo.Update(existingPerson);

            //3. Just attache the new entities with existing ids not working..
            var person = new Person();
            person.Id = 7;
            person.Name = "Bush";
            _personRepo.Attach(person);

            var passport = new Passport() { Id = 8 };
            _passportRepo.Attach(passport);
            person.Passport = passport;
            _personRepo.Update(person);
            
            _unitOfWork.Save();
        }
    }
}