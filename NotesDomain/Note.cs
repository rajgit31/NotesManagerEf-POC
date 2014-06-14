using System.Collections.Generic;

namespace NotesDomain
{
    public class Note : BaseEntity
    {
        private IList<NoteVersion> _noteVersions;

        public Note()
        {
            _noteVersions = new List<NoteVersion>();
        }

        public string Title { get; set; }
        public string Description { get; set; }

        public virtual IList<NoteVersion> NoteVersions
        {
            get { return _noteVersions; } 
            set { _noteVersions = value; }
        }
    }
}
