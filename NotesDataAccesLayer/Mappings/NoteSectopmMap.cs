using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class NoteSectopmMap : EntityMap<NoteSection>
    {
        public NoteSectopmMap()
        {
            this.Property(x => x.SectionName);
            this.Property(x => x.SectionColor);
        }
    }
}