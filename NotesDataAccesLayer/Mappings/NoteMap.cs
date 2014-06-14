using NotesDomain;

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

    public class NoteVersionMap : EntityMap<NoteVersion>
    {
        public NoteVersionMap()
        {
            this.Property(x => x.Version);
            this.Property(x => x.Name);
        }
    }
}