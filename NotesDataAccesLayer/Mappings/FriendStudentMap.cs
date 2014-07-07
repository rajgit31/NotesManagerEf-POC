using System.Data.Entity.ModelConfiguration;
using NotesDomain.Entities.OneToOne;

namespace NotesDataAccesLayer.Mappings
{
    public class FriendStudentMap : EntityTypeConfiguration<FriendStudentMapping>
    {
        public FriendStudentMap()
        {
            this.HasKey(x => x.Id);
        }
    }
}