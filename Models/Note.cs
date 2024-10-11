namespace CSharpAspCoreAdoPg.Models
{
    using System.Text.Json.Serialization;

    public class Note
    {
        public int Id { get; set; }

        [JsonPropertyName("noteText")]
        public string NoteText { get; set; } = string.Empty;

        [JsonPropertyName("applicationId")]
        public int ApplicationId { get; set; }
    }
}
