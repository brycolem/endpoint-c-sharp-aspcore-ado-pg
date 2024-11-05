namespace CSharpAspCoreAdoPg.Models
{
  public class ApiOverview
  {
    public required Dictionary<string, Dictionary<string, string>> _links { get; set; }

    public ApiOverview()
    {
      _links = new Dictionary<string, Dictionary<string, string>>();
    }
  }
}
