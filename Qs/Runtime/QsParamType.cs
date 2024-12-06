namespace Qs.Runtime;

/// <summary>
/// Types of paramteters that QsFunction accept.
/// </summary>
public enum QsParamType
{
    /// <summary>
    /// Raw parameter will enter as it is to the parameter.
    /// </summary>
    Raw,


    /// <summary>
    /// Text parameter.
    /// </summary>
    Text,

    /// <summary>
    /// Value parameter should be evaluated before going to the parameter.
    /// </summary>
    Value,

    /// <summary>
    /// Function parameter is a pointer to another function.
    /// </summary>
    Function
}