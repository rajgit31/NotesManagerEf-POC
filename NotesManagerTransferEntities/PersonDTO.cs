namespace NotesManagerTransferEntities
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual PassportDTO PassportDTO { get; set; }
    }

    public class PassportDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Nationality { get; set; }
    }
}