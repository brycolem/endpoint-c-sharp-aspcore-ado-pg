using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace CSharpAspCoreAdoPg.Tests.Mocks
{
  public class MockDataParameterCollection : DbParameterCollection
  {
    private readonly List<DbParameter> _parameters = new List<DbParameter>();

    public override int Count => _parameters.Count;

    public override object SyncRoot => null;

    public override int Add(object value)
    {
      _parameters.Add((DbParameter)value);
      return _parameters.Count - 1;
    }

    public override void AddRange(Array values)
    {
      foreach (var value in values)
      {
        Add(value);
      }
    }

    public override void Clear() => _parameters.Clear();

    public override bool Contains(object value) => _parameters.Contains((DbParameter)value);

    public override bool Contains(string value) => _parameters.Exists(p => p.ParameterName == value);

    public override void CopyTo(Array array, int index)
    {
      _parameters.ToArray().CopyTo(array, index);
    }

    public override IEnumerator GetEnumerator() => _parameters.GetEnumerator();

    protected override DbParameter GetParameter(int index) => _parameters[index];

    protected override DbParameter? GetParameter(string parameterName)
    {
      return _parameters.Find(p => p.ParameterName == parameterName);
    }

    public override int IndexOf(object value) => _parameters.IndexOf((DbParameter)value);

    public override int IndexOf(string parameterName) => _parameters.FindIndex(p => p.ParameterName == parameterName);

    public override void Insert(int index, object value) => _parameters.Insert(index, (DbParameter)value);

    public override bool IsFixedSize => false;

    public override bool IsReadOnly => false;

    public override bool IsSynchronized => false;

    public override void Remove(object value) => _parameters.Remove((DbParameter)value);

    public override void RemoveAt(int index) => _parameters.RemoveAt(index);

    public override void RemoveAt(string parameterName)
    {
      var index = IndexOf(parameterName);
      if (index >= 0)
        _parameters.RemoveAt(index);
    }

    protected override void SetParameter(int index, DbParameter value) => _parameters[index] = value;

    protected override void SetParameter(string parameterName, DbParameter value)
    {
      var index = IndexOf(parameterName);
      if (index >= 0)
        _parameters[index] = value;
      else
        _parameters.Add(value);
    }
  }
}