using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using CSharpAspCoreAdoPg.Controllers;
using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSharpAspCoreAdoPg.Tests.Controllers
{
  public class ApplicationsControllerTests
  {
    private readonly Mock<IApplicationService> _mockApplicationService;
    private readonly ApplicationsController _controller;

    public ApplicationsControllerTests()
    {
      _mockApplicationService = new Mock<IApplicationService>();
      _controller = new ApplicationsController(_mockApplicationService.Object);
    }

    [Fact]
    public async Task GetApplications_ReturnsOkResult_WithListOfApplications()
    {
      var mockApplications = new List<Application>
            {
                new Application { Id = 1, Employer = "Employer1", Title = "Title1", Link = "http://example.com/1", CompanyId = 101 },
                new Application { Id = 2, Employer = "Employer2", Title = "Title2", Link = "http://example.com/2", CompanyId = 102 }
            };
      _mockApplicationService.Setup(service => service.GetApplicationsAsync()).ReturnsAsync(mockApplications);

      var result = await _controller.GetApplications();

      var okResult = Assert.IsType<OkObjectResult>(result);
      var returnedApplications = Assert.IsAssignableFrom<IEnumerable<Application>>(okResult.Value);
      Assert.Equal(2, ((List<Application>)returnedApplications).Count);
    }

    [Fact]
    public async Task AddApplication_ReturnsOkResult_WithSuccessMessage()
    {
      var newApplication = new Application { Employer = "Employer3", Title = "Title3", Link = "http://example.com/3", CompanyId = 103 };
      _mockApplicationService.Setup(service => service.AddApplicationAsync(newApplication)).Returns(Task.CompletedTask);

      var result = await _controller.AddApplication(newApplication);

      var okResult = Assert.IsType<OkObjectResult>(result);
      Assert.Equal($"Application '{newApplication.Title}' added successfully.", okResult.Value);
    }
  }
}
