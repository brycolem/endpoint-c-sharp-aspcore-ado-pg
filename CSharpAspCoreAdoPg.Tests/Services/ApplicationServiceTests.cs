using Xunit;
using CSharpAspCoreAdoPg.Services;
using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Tests.Mocks;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

namespace CSharpAspCoreAdoPg.Tests.Services
{
  public class ApplicationServiceTests
  {
    [Fact]
    public async Task GetApplicationsAsync_ReturnsApplications()
    {
      var mockRecords = new List<IDictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    { "id", 1 },
                    { "employer", "Mock Employer" },
                    { "title", "Mock Title" },
                    { "link", "http://mocklink.com" },
                    { "company_id", 1 },
                    { "notes", "[{\"id\":1,\"noteText\":\"Mock note\",\"applicationId\":1}]" }
                }
            };

      var mockDataReader = new MockDataReader(mockRecords);

      var mockCommand = new MockDbCommand
      {
        ExecuteReaderAsyncFunc = () => Task.FromResult<DbDataReader>(mockDataReader)
      };

      var mockConnection = new MockDbConnection
      {
        Command = mockCommand
      };

      var mockConnectionFactory = new MockDbConnectionFactory(mockConnection);

      var service = new ApplicationService(mockConnectionFactory);

      var applications = await service.GetApplicationsAsync();

      Assert.NotNull(applications);
      var applicationList = applications.ToList();
      Assert.Single(applicationList);
      var application = applicationList[0];
      Assert.Equal(1, application.Id);
      Assert.Equal("Mock Employer", application.Employer);
      Assert.Equal("Mock Title", application.Title);
      Assert.Equal("http://mocklink.com", application.Link);
      Assert.Equal(1, application.CompanyId);
      Assert.NotNull(application.Notes);
      Assert.Single(application.Notes);
      Assert.Equal("Mock note", application.Notes[0].NoteText);
    }

    [Fact]
    public async Task AddApplicationAsync_AddsApplication()
    {
      var application = new Application
      {
        Employer = "Test Employer",
        Title = "Test Title",
        Link = "http://testlink.com",
        CompanyId = 2
      };

      var mockCommand = new MockDbCommand
      {
        ExecuteScalarAsyncFunc = () => Task.FromResult<object>(1)
      };

      var mockConnection = new MockDbConnection
      {
        Command = mockCommand
      };

      var mockConnectionFactory = new MockDbConnectionFactory(mockConnection);

      var service = new ApplicationService(mockConnectionFactory);

      await service.AddApplicationAsync(application);

      Assert.Equal(1, application.Id);
    }
  }
}
