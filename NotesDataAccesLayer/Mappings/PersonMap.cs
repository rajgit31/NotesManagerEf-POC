using System.Data.Entity.ModelConfiguration;
using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class PersonMap : EntityTypeConfiguration<Person>
    {
        public PersonMap()
        {
            HasKey(x => x.Id);
            HasRequired(a => a.Passport);
        }
    }

    public class PassportMap : EntityTypeConfiguration<Passport>
    {
        public PassportMap()
        {
            HasKey(x => x.Id);
        }
    }
}