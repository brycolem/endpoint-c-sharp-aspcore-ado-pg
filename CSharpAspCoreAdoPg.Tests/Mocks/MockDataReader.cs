using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Threading;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDataReader : DbDataReader
  {
    private readonly IEnumerator<IDictionary<string, object>> _enumerator;
    private IDictionary<string, object> Current => _enumerator.Current;

    public MockDataReader(IEnumerable<IDictionary<string, object>> records)
    {
      _enumerator = records.GetEnumerator();
    }

    public override bool Read()
    {
      return _enumerator.MoveNext();
    }

    public override int FieldCount => Current.Count;

    public override object this[int ordinal] => Current[GetName(ordinal)];
    public override object this[string name] => Current[name];

    public override string GetName(int ordinal) => new List<string>(Current.Keys)[ordinal];

    public override int GetOrdinal(string name) => new List<string>(Current.Keys).IndexOf(name);

    public override object GetValue(int ordinal) => this[ordinal];

    public override int GetValues(object[] values)
    {
      int count = Math.Min(values.Length, FieldCount);
      for (int i = 0; i < count; i++)
      {
        values[i] = GetValue(i);
      }
      return count;
    }

    public override bool HasRows => true;
    public override bool IsClosed => false;
    public override int RecordsAffected => -1;
    public override int Depth => 0;

    public override bool GetBoolean(int ordinal) => Convert.ToBoolean(GetValue(ordinal));
    public override byte GetByte(int ordinal) => Convert.ToByte(GetValue(ordinal));
    public override long GetBytes(int ordinal, long dataOffset, byte[]? buffer, int bufferOffset, int length) => throw new NotImplementedException();
    public override char GetChar(int ordinal) => Convert.ToChar(GetValue(ordinal));
    public override long GetChars(int ordinal, long dataOffset, char[]? buffer, int bufferOffset, int length) => throw new NotImplementedException();
    public override string GetDataTypeName(int ordinal) => GetFieldType(ordinal).Name;
    public override DateTime GetDateTime(int ordinal) => Convert.ToDateTime(GetValue(ordinal));
    public override decimal GetDecimal(int ordinal) => Convert.ToDecimal(GetValue(ordinal));
    public override double GetDouble(int ordinal) => Convert.ToDouble(GetValue(ordinal));
    public override Type GetFieldType(int ordinal) => GetValue(ordinal)?.GetType() ?? typeof(object);
    public override float GetFloat(int ordinal) => Convert.ToSingle(GetValue(ordinal));
    public override Guid GetGuid(int ordinal)
    {
      var value = GetValue(ordinal);
      if (value == null)
        throw new InvalidOperationException("Cannot get string from a null value.");
      return Guid.Parse(value.ToString() ?? "");
    }
    public override short GetInt16(int ordinal) => Convert.ToInt16(GetValue(ordinal));
    public override int GetInt32(int ordinal) => Convert.ToInt32(GetValue(ordinal));
    public override long GetInt64(int ordinal) => Convert.ToInt64(GetValue(ordinal));
    public override string GetString(int ordinal)
    {
      var value = GetValue(ordinal);
      if (value == null)
        throw new InvalidOperationException("Cannot get string from a null value.");
      return value.ToString() ?? "";
    }
    public override bool IsDBNull(int ordinal) => GetValue(ordinal) == null || GetValue(ordinal) == DBNull.Value;

    public override bool NextResult() => false;

    public override Task<bool> ReadAsync(CancellationToken cancellationToken)
    {
      return Task.FromResult(Read());
    }

    public override Task<bool> NextResultAsync(CancellationToken cancellationToken)
    {
      return Task.FromResult(NextResult());
    }

    public override void Close() { /* No operation needed */ }

    public override DataTable GetSchemaTable()
    {
      throw new NotImplementedException();
    }

    public override IEnumerator GetEnumerator()
    {
      return _enumerator;
    }
  }
}
