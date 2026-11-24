namespace Qs.Types.Attributes;

/// <summary>
/// Quantity System Function Attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class QsFunctionAttribute(string functionName) : Attribute
{
    public string FunctionName { get; } = functionName;

    public bool DefaultScopeFunction { get; set; } = false;
}
