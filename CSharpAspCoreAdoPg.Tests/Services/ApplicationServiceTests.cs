using Xunit;
using Moq;
using CSharpAspCoreAdoPg.Models;
using CSharpAspCoreAdoPg.Repositories;
using CSharpAspCoreAdoPg.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class ApplicationServiceTests
{
  private readonly Mock<IApplicationRepository> _mockRepository;
  private readonly ApplicationService _applicationService;

  public ApplicationServiceTests()
  {
    _mockRepository = new Mock<IApplicationRepository>();
    _applicationService = new ApplicationService(_mockRepository.Object);
  }

  [Fact]
  public async Task GetApplicationsAsync_ShouldReturnApplications()
  {
    // Arrange
    var mockApplications = new List<Application>
        {
            new Application { Id = 1, Employer = "Employer1", Title = "Title1" },
            new Application { Id = 2, Employer = "Employer2", Title = "Title2" }
        };
    _mockRepository.Setup(repo => repo.GetAllApplicationsAsync()).ReturnsAsync(mockApplications);

    // Act
    var result = await _applicationService.GetApplicationsAsync();

    // Assert
    Assert.NotNull(result);
    Assert.Equal(2, result.Count());
  }

  [Fact]
  public async Task GetApplicationAsync_ShouldReturnApplication_WhenExists()
  {
    // Arrange
    var mockApplication = new Application { Id = 1, Employer = "Employer1", Title = "Title1" };
    _mockRepository.Setup(repo => repo.GetApplicationByIdAsync(1)).ReturnsAsync(mockApplication);

    // Act
    var result = await _applicationService.GetApplicationAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result?.Id);
    Assert.Equal("Employer1", result?.Employer);
  }

  [Fact]
  public async Task GetApplicationAsync_ShouldReturnNull_WhenNotExists()
  {
    // Arrange
    _mockRepository.Setup(repo => repo.GetApplicationByIdAsync(99)).ReturnsAsync((Application?)null);

    // Act
    var result = await _applicationService.GetApplicationAsync(99);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task AddApplicationAsync_ShouldInvokeRepositoryMethod()
  {
    // Arrange
    var newApplication = new Application { Employer = "New Employer", Title = "New Title" };

    // Act
    await _applicationService.AddApplicationAsync(newApplication);

    // Assert
    _mockRepository.Verify(repo => repo.AddApplicationAsync(newApplication), Times.Once);
  }
}
