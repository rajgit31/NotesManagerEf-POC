using System.Collections.Generic;

namespace NotesDomain.Entities
{
    public class FillerForm : BaseEntity
    {
        public string Name { get; set; }
        public IList<QuestionAnswerMapping> QuestionAnswerMappings { get; set; }
    }

    public class QuestionAnswerMapping : BaseEntity
    {
        public string Client { get; set; }
        
        public Note Note { get; set; }
        //public NoteVersion NoteVersion { get; set; }
        //public NoteSection NoteSection { get; set; }

        public FillerForm FillerForm { get; set; }
        public int FK_FillerFormId { get; set; }
    }
}