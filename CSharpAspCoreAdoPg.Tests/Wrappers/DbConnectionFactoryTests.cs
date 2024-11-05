using Xunit;
using System;
using System.Data;
using Npgsql;
using CSharpAspCoreAdoPg.Wrappers;

namespace CSharpAspCoreAdoPg.Tests.Wrappers
{
  public class DbConnectionFactoryTests
  {
    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenConnectionStringIsNull()
    {
      Assert.Throws<ArgumentNullException>(() => new DbConnectionFactory(null));
    }

    [Fact]
    public void CreateConnection_ReturnsNpgsqlConnection_WithCorrectConnectionString()
    {
      var connectionString = "Host=localhost;Database=testdb;Username=testuser;Password=testpassword";
      var factory = new DbConnectionFactory(connectionString);

      var connection = factory.CreateConnection();

      Assert.NotNull(connection);
      Assert.IsType<NpgsqlConnection>(connection);
      Assert.Equal(connectionString, connection.ConnectionString);
    }
  }
}
