using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class FillerFormMap : EntityMap<FillerForm>
    {
        public FillerFormMap()
        {
            this.Property(x => x.Name);
            this.HasMany(x => x.QuestionAnswerMappings)
                .WithRequired(r => r.FillerForm)
                .HasForeignKey(x => x.FK_FillerFormId)
                .WillCascadeOnDelete(true);
        }
    }
}