namespace Qs.Runtime;

/// <summary>
/// Function Parameter Information Attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class QsParamInfoAttribute(QsParamType parameterType) : Attribute
{
    public QsParamType ParameterType { get; set; } = parameterType;
}