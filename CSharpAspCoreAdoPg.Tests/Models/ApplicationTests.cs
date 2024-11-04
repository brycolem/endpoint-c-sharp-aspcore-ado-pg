using Xunit;
using CSharpAspCoreAdoPg.Models;

namespace CSharpAspCoreAdoPg.Tests.Models
{
  public class ApplicationTests
  {
    [Fact]
    public void Application_Should_Initialize_With_ExpectedValues()
    {
      var app = new Application
      {
        Id = 1,
        Employer = "Test Employer",
        Title = "Test Title",
        Link = "http://testlink.com",
        CompanyId = 123
      };

      Assert.Equal(1, app.Id);
      Assert.Equal("Test Employer", app.Employer);
      Assert.Equal("Test Title", app.Title);
      Assert.Equal("http://testlink.com", app.Link);
      Assert.Equal(123, app.CompanyId);
      Assert.NotNull(app.Notes);
      Assert.Empty(app.Notes);
    }
  }
}
