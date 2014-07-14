namespace NotesDomain.Entities
{
    public class FriendStudentMapping : BaseEntity
    {
        public int Id { get; set; }
        public virtual Student Student { get; set; }
    }
}