namespace Qs.Types.Attributes;

/// <summary>
/// Quantity System Function Attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class QsFunctionAttribute(string functionName) : Attribute
{
    public string FunctionName { get; private set; } = functionName;
    public bool DefaultScopeFunction { get; set; } = false;
}