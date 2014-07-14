namespace NotesDomain.Entities
{
    public class QuestionAnswerMapping : BaseEntity
    {
        public string Client { get; set; }

        //one to one
        public Note Note { get; set; }
        public AcpAnswer AcpAnswer { get; set; }

        //one to many
        public FillerForm FillerForm { get; set; }
        public int FK_FillerFormId { get; set; }
        

    }

    public class AcpAnswer : BaseEntity
    {
        public string AnswerText { get; set; }

        //navigation property
        public virtual AcpQuestionListItem AcpQuestionListItem { get; set; }

        public int FK_AcpQuestionListItemID { get; set; }
    }

    public class AcpQuestionListItem : BaseEntity
    {
        public string Name { get; set; }
        //public AcpListItem AcpListItem { get; set; }
        //public virtual AcpAnswer AcpAnswer { get; set; }
    }

    //public class AcpListItem : BaseEntity
    //{
    //    public string ItemText { get; set; }    
    //}


}