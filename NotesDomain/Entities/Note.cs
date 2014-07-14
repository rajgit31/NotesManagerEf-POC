using System.Collections.Generic;

namespace NotesDomain.Entities
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

        public IList<NoteVersion> NoteVersions
        {
            get { return _noteVersions; } 
            set { _noteVersions = value; }
        }
    }

    public class FillerForm : BaseEntity
    {
        public string Name { get; set; }
        public IList<QuestionAnswerMapping> QuestionAnswerMappings { get; set; }
    }
}
