using Xunit;
using CSharpAspCoreAdoPg.Models;
using System.Collections.Generic;

namespace CSharpAspCoreAdoPg.Tests.Models
{
  public class ApiOverviewTests
  {
    [Fact]
    public void ApiOverview_CanSetAndGetLinks()
    {
      var links = new Dictionary<string, Dictionary<string, string>>
    {
        { "self", new Dictionary<string, string> { { "href", "/" }, { "method", "GET" } } },
        { "swagger_docs", new Dictionary<string, string> { { "href", "/docs" }, { "method", "GET" } } },
        { "get_all_applications", new Dictionary<string, string> { { "href", "/application" }, { "method", "GET" } } },
        { "get_application", new Dictionary<string, string> { { "href", "/application/{application_id}" }, { "method", "GET" } } },
        { "create_application", new Dictionary<string, string> { { "href", "/application" }, { "method", "POST" } } },
        { "update_application", new Dictionary<string, string> { { "href", "/application/{application_id}" }, { "method", "PUT" } } },
        { "patch_application", new Dictionary<string, string> { { "href", "/application/{application_id}" }, { "method", "PATCH" } } },
        { "delete_application", new Dictionary<string, string> { { "href", "/application/{application_id}" }, { "method", "DELETE" } } }
    };

      var apiOverview = new ApiOverview
      {
        _links = links
      };

      Assert.Equal(links, apiOverview._links);
    }
  }
}
