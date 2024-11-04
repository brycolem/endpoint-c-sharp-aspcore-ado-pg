using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDbConnection : DbConnection
  {
    public DbCommand Command { get; set; }

    public override string ConnectionString { get; set; }

    public override string Database => "MockDatabase";

    public override string DataSource => "MockDataSource";

    public override string ServerVersion => "MockVersion";

    public override ConnectionState State => ConnectionState.Open;

    public override void ChangeDatabase(string databaseName) { /* No operation needed */ }

    public override void Close() { /* No operation needed */ }

    public override void Open() { /* No operation needed */ }

    public override Task OpenAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }

    protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
    {
      return null;
    }

    protected override DbCommand CreateDbCommand()
    {
      return Command;
    }

    protected override void Dispose(bool disposing)
    {
      // Dispose resources if necessary
    }
  }
}
