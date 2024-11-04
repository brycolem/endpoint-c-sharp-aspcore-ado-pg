using System.Data;
using System.Data.Common;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDbParameter : DbParameter
  {
    public override DbType DbType { get; set; }
    public override ParameterDirection Direction { get; set; } = ParameterDirection.Input;
    public override bool IsNullable { get; set; } = true;

    // Non-nullable properties initialized to avoid warnings
    public override string ParameterName { get; set; } = string.Empty;
    public override string SourceColumn { get; set; } = string.Empty;

    public override DataRowVersion SourceVersion { get; set; } = DataRowVersion.Current;

    // Nullable property
    public override object? Value { get; set; }

    public override int Size { get; set; }
    public override bool SourceColumnNullMapping { get; set; }

    public override void ResetDbType()
    {
      // No operation needed
    }
  }
}
