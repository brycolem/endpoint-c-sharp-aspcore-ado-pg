namespace CSharpAspCoreAdoPg.Models
{
    public class Application
    {
        public int Id { get; set; }
        public string Employer { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public int CompanyId { get; set; }
        public List<Note> Notes { get; set; }

        public Application()
        {
            Notes = new List<Note>();
        }
    }
}
