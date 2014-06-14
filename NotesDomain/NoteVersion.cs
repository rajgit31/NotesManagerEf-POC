namespace NotesDomain
{
    public class NoteVersion : BaseEntity
    {
        public NoteVersion()
        {
        }

        public int Version { get; set; }
        public string Name { get; set; }
        
        public virtual Note Note { get; set; }
        public int FK_NoteId { get; set; }
    }
}