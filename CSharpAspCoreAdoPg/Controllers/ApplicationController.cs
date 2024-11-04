using Microsoft.AspNetCore.Mvc;
using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Services;

namespace CSharpAspCoreAdoPg.Controllers
{
    [ApiController]
    [Route("application")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetApplications()
        {
            var applications = await _applicationService.GetApplicationsAsync();
            return Ok(applications);
        }

        [HttpPost]
        public async Task<IActionResult> AddApplication([FromBody] Application application)
        {
            await _applicationService.AddApplicationAsync(application);
            return Ok($"Application '{application.Title}' added successfully.");
        }
    }
}
