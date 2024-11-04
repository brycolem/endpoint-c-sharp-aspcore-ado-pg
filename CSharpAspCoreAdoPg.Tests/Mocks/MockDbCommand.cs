using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDbCommand : DbCommand
  {
    public Func<Task<DbDataReader>>? ExecuteReaderAsyncFunc { get; set; }
    public Func<Task<object>>? ExecuteScalarAsyncFunc { get; set; }

    public override string? CommandText { get; set; } = null;
    public override int CommandTimeout { get; set; }
    public override CommandType CommandType { get; set; } = CommandType.Text;

    protected override DbConnection? DbConnection { get; set; }
    protected override DbParameterCollection DbParameterCollection { get; } = new MockDataParameterCollection();
    protected override DbTransaction? DbTransaction { get; set; }

    public override UpdateRowSource UpdatedRowSource { get; set; }
    public override bool DesignTimeVisible { get; set; }

    public override void Cancel() { /* No operation needed */ }

    public override int ExecuteNonQuery() => 0;
    [return: System.Diagnostics.CodeAnalysis.NotNull]
    protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
    {
      throw new NotImplementedException();
    }

    public override Task<int> ExecuteNonQueryAsync(CancellationToken cancellationToken)
    {
      return Task.FromResult(0);
    }

    protected override Task<DbDataReader>? ExecuteDbDataReaderAsync(CommandBehavior behavior, CancellationToken cancellationToken)
    {
      if (ExecuteReaderAsyncFunc is null)
        throw new InvalidOperationException("ExecuteReaderAsyncFunc is not set.");
      return ExecuteReaderAsyncFunc();
    }

    public override object? ExecuteScalar()
    {
      throw new NotImplementedException();
    }

    public override Task<object>? ExecuteScalarAsync(CancellationToken cancellationToken)
    {
      if (ExecuteScalarAsyncFunc is null)
        throw new InvalidOperationException("ExecuteScalarAsyncFunc is not set.");
      return ExecuteScalarAsyncFunc();
    }

    public override void Prepare() { /* No operation needed */ }

    protected override DbParameter CreateDbParameter()
    {
      return new MockDbParameter();
    }
  }
}
