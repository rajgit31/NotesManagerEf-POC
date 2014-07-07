using System.Data.Entity.ModelConfiguration;
using NotesDomain.Entities.OneToOne;

namespace NotesDataAccesLayer.Mappings
{
    public class StudentMap : EntityTypeConfiguration<Student> 
    {
        public StudentMap()
        {
            //Primary Key of any entity
            this.HasKey(x => x.Id);
            this.Property(x => x.Name);

        }
    }
}