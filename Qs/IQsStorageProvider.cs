namespace Qs;

public interface IQsStorageProvider : IDisposable
{
    IEnumerable<KeyValuePair<string, object>> GetItems();
    IEnumerable<string> GetKeys();
    IEnumerable<object> GetValues();

    bool HasValue(string variable);
    object GetValue(string variable);
    void SetValue(string variable, object value);
    bool TryGetValue(string variable, out object q);
    bool DeleteValue(string variable);
    void Clear();
}