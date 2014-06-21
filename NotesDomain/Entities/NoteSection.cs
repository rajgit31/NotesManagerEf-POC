using System;

namespace NotesDomain.Entities
{
    public class NoteSection : BaseEntity
    {
        public string SectionName { get; set; }
        public string SectionColor { get; set; }

        public virtual NoteVersion NoteVersion { get; set; }
        public int FK_NoteVersionId { get; set; }
        public NoteSectionType SectionType { get; set; }

        public Guid? AddtionalId { get; set; }
    }
}