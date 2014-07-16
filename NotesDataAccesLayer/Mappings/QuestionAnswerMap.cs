using NotesDomain.Entities;

namespace NotesDataAccesLayer.Mappings
{
    public class QuestionAnswerMap : EntityMap<QuestionAnswerMapping>
    {
        public QuestionAnswerMap()
        {
            this.Property(x => x.Client);
        }
    }

    public class AcpAnswerMap : EntityMap<AcpAnswer>
    {
        public AcpAnswerMap()
        {
            this.Property(x => x.AnswerText);
            this.HasRequired(x => x.AcpQuestionListItem);
        }
    }

    public class AcpQuestionListItemMap : EntityMap<AcpQuestionListItem>
    {
        public AcpQuestionListItemMap()
        {
            
        }
    }


    //public class AcpListItemMap : EntityMap<AcpListItem>
    //{
    //    public AcpListItemMap()
    //    {
    //        this.Property(x => x.ItemText);
    //    }
    //}
}