namespace Qs.Types;

/// <summary>
/// Tuple Element
/// </summary>
public struct QsTupleValue
{
    public int Id;
    public string Name;

    /// <summary>
    /// Holds original source value that wasn't converted into Native Qs Value yet
    /// </summary>
    private Object? _LazyValue;
    public Object? LazyValue => _LazyValue;

    public QsValue Value;

    /// <summary>
    /// Set the tuple value to a value that will be evaluated when value accessed first time
    /// </summary>
    /// <param name="value"></param>
    public void SetLazyValue(Object value)
    {
        _LazyValue = value;
    }

    public bool IsLazyValue
    {
        get
        {
            return _LazyValue != null;
        }
    }

    public QsTupleValue(QsValue value)
    {
        Id = 0;
        Name = string.Empty;
        Value = value;

        _LazyValue = null;
    }

    public QsTupleValue(string name, QsValue value)
    {
        Id = 0;
        Name = name;
        Value = value;

        _LazyValue = null;
    }

    public QsTupleValue(int id, string name, QsValue value)
    {
        Id = id;
        Name = name;
        Value = value;

        _LazyValue = null;
    }

    public QsTupleValue(int id, QsValue value)
    {
        Id = id;
        Name = string.Empty;
        Value = value;

        _LazyValue = null;
    }
}