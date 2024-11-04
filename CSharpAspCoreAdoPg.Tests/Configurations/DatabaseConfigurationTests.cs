using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using CSharpAspCoreAdoPg.Configurations;

namespace CSharpAspCoreAdoPg.Tests.Configurations
{
  public class DatabaseConfigurationTests
  {
    [Fact]
    public void DatabaseConfiguration_ShouldInitialize_WithCorrectConnectionString()
    {
      var mockDatabaseSection = new Mock<IConfigurationSection>();
      mockDatabaseSection.SetupGet(m => m.Value).Returns("TestDatabase");

      var mockUserSection = new Mock<IConfigurationSection>();
      mockUserSection.SetupGet(m => m.Value).Returns("TestUser");

      var mockPasswordSection = new Mock<IConfigurationSection>();
      mockPasswordSection.SetupGet(m => m.Value).Returns("TestPassword");

      var configurationMock = new Mock<IConfiguration>();
      configurationMock.Setup(c => c.GetSection("DATABASE")).Returns(mockDatabaseSection.Object);
      configurationMock.Setup(c => c.GetSection("DB_USER")).Returns(mockUserSection.Object);
      configurationMock.Setup(c => c.GetSection("DB_PWD")).Returns(mockPasswordSection.Object);

      var dbConfig = new DatabaseConfiguration(configurationMock.Object);

      Assert.Equal("Server=localhost;Database=TestDatabase;User Id=TestUser;Password=TestPassword;Pooling=true;MaxPoolSize=100;", dbConfig.ConnectionString);
    }
  }
}
