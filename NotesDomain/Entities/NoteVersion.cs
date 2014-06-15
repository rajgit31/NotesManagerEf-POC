using System.Collections.Generic;

namespace NotesDomain.Entities
{
    public class NoteVersion : BaseEntity
    {
        private IList<NoteSection> _noteSections;
        public NoteVersion()
        {
            _noteSections = new List<NoteSection>();
        }

        public int Version { get; set; }
        public string Name { get; set; }
        
        public virtual Note Note { get; set; }
        public int FK_NoteId { get; set; }
        
        public virtual IList<NoteSection> NoteSection { get; set; }
    }
}