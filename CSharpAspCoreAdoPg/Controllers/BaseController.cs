using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CSharpAspCoreAdoPg.Models;

namespace CSharpAspCoreAdoPg.Controllers
{
  [ApiController]
  [Route("/")]
  public class BaseController : ControllerBase
  {
    [HttpGet]
    public IActionResult GetApiOverview()
    {
      string baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
      baseUrl = baseUrl.EndsWith("/") ? baseUrl.Substring(0, baseUrl.Length - 1) : baseUrl;

      var links = new Dictionary<string, Dictionary<string, string>>
            {
                { "self", CreateLink($"{baseUrl}/", "GET") },
                { "swagger_docs", CreateLink($"{baseUrl}/docs", "GET") },
                { "get_all_applications", CreateLink($"{baseUrl}/application", "GET") },
                { "get_application", CreateLink($"{baseUrl}/application/{{application_id}}", "GET") },
                { "create_application", CreateLink($"{baseUrl}/application", "POST") },
                { "update_application", CreateLink($"{baseUrl}/application/{{application_id}}", "PUT") },
                { "patch_application", CreateLink($"{baseUrl}/application/{{application_id}}", "PATCH") },
                { "delete_application", CreateLink($"{baseUrl}/application/{{application_id}}", "DELETE") }
            };

      return Ok(new ApiOverview { _links = links });
    }

    private Dictionary<string, string> CreateLink(string href, string method)
    {
      return new Dictionary<string, string>
            {
                { "href", href },
                { "method", method }
            };
    }
  }
}
