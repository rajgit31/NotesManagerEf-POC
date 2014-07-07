using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class NoteSectionMap : EntityMap<NoteSection>
    {
        public NoteSectionMap()
        {
            this.Property(x => x.SectionName);
            this.Property(x => x.SectionColor);
        }
    }
}