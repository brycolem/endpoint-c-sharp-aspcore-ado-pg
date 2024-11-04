using System.Data;
using CSharpAspCoreAdoPg.Wrappers;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDbConnectionFactory : IDbConnectionFactory
  {
    private readonly IDbConnection _connection;

    public MockDbConnectionFactory(IDbConnection connection)
    {
      _connection = connection;
    }

    public IDbConnection CreateConnection()
    {
      return _connection;
    }
  }
}
