using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using CSharpAspCoreAdoPg.Services;
using CSharpAspCoreAdoPg.Models;
using System.Collections.Generic;

namespace CSharpAspCoreAdoPg.Tests
{
    public class ProgramTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProgramTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IApplicationService));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    var mockAppService = new Mock<IApplicationService>();
                    mockAppService.Setup(s => s.GetApplicationsAsync())
                        .ReturnsAsync(new List<Application>
                        {
                            new Application { Id = 1, Title = "Test App", Employer = "Test Employer" }
                        });

                    services.AddScoped(_ => mockAppService.Object);
                });
            });
        }

        [Fact]
        public async Task ApplicationStartsAndReturnsSuccessStatusCode()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/application");

            response.EnsureSuccessStatusCode();
        }
    }
}
