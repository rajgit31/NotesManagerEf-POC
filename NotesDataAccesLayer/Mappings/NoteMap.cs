using System.Data.Entity.ModelConfiguration;
using NotesDomain;
using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class NoteMap : EntityMap<Note>
    {
        public NoteMap()
        {
            this.Property(x => x.Title);
            this.Property(x => x.Description);
            this.HasMany(x => x.NoteVersions)
                .WithRequired(r => r.Note)
                .HasForeignKey(x => x.FK_NoteId)
                .WillCascadeOnDelete(true);
        }
    }


    public class FriendStudentMap : EntityTypeConfiguration<FriendStudentMapping>
    {
        public FriendStudentMap()
        {
            this.HasKey(x => x.Id);
        }
    }

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