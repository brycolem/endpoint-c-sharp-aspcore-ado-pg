namespace CSharpAspCoreAdoPg.Models
{
    using System.Text.Json.Serialization;

    public class Application
    {
        public int Id { get; set; }
        public string Employer { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;

        [JsonPropertyName("companyId")]
        public int CompanyId { get; set; }
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
