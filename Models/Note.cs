namespace CSharpAspCoreAdoPg.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteText { get; set; }
        public int ApplicationId { get; set; }
    }
}
