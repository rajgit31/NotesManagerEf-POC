using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class NoteVersionMap : EntityMap<NoteVersion>
    {
        public NoteVersionMap()
        {
            this.Property(x => x.Version);
            this.Property(x => x.Name);
            this.HasMany(x => x.NoteSection)
                .WithRequired(r => r.NoteVersion)
                .HasForeignKey(x => x.FK_NoteVersionId)
                .WillCascadeOnDelete(true);
        }
    }
}