using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class NoteVersionMap : EntityMap<NoteVersion>
    {
        public NoteVersionMap()
        {
            this.Property(x => x.Version);
            this.Property(x => x.Name);
        }
    }
}