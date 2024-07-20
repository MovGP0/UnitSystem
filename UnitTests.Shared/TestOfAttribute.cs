namespace UnitTests.Shared;

/// <summary>
/// Attribute to describe the object or method under test.
/// </summary>
/// <remarks>
/// This attribute can be applied to test classes or test methods to indicate what is being tested.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class TestOfAttribute : Attribute
{
    /// <summary>
    /// Gets the name of the object or method under test.
    /// </summary>
    /// <value>
    /// The name of the object or method under test.
    /// </value>
    public string Name { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestOfAttribute"/> class using a type.
    /// </summary>
    /// <param name="type">The type of the object under test.</param>
    /// <remarks>
    /// The name of the type will be used as the name of the object under test.
    /// </remarks>
    public TestOfAttribute(Type type)
    {
        Name = type.Name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestOfAttribute"/> class using a name.
    /// </summary>
    /// <param name="name">The name of the object or method under test.</param>
    public TestOfAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestOfAttribute"/> class using a type and a name.
    /// </summary>
    /// <param name="type">The type of the object under test.</param>
    /// <param name="name">The name of the method under test.</param>
    /// <remarks>
    /// The name of the object under test will be a combination of the type name and the method name.
    /// </remarks>
    public TestOfAttribute(Type type, string name)
    {
        Name = $"{type.Name}.{name}";
    }
}
