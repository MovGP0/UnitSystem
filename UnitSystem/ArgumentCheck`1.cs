namespace UnitSystem;

/// <summary>
/// Provides a mechanism to check the validity of arguments and throw appropriate exceptions if the checks fail.
/// </summary>
/// <typeparam name="T">The type of the argument to check.</typeparam>
internal class ArgumentCheck<T>
{
    private readonly T _argument;
    private readonly string _paramName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentCheck{T}"/> class with the specified argument and parameter name.
    /// </summary>
    /// <param name="argument">The argument to check.</param>
    /// <param name="paramName">The name of the parameter representing the argument.</param>
    public ArgumentCheck(T argument, string paramName)
    {
        _argument = argument;
        _paramName = paramName;
    }

    /// <summary>
    /// Checks if the argument is not null or, in case of a string, not null or empty.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when the argument is null or, in case of a string, null or empty.</exception>
    /// <example>
    /// <code>
    /// var check = new ArgumentCheck&lt;string&gt;("test", "paramName");
    /// check.IsNotNull();
    /// // No exception is thrown since the argument is not null or empty.
    /// </code>
    /// </example>
    public void IsNotNull()
    {
        if (_argument == null || (_argument is string argument && string.IsNullOrEmpty(argument)))
        {
            throw new ArgumentNullException(_paramName);
        }
    }
}
