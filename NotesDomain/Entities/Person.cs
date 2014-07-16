namespace NotesDomain.Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Passport Passport { get; set; }
    }

    public class Passport
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Nationality { get; set; }
    }
}