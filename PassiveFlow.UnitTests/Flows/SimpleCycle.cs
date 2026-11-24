namespace PassiveFlow.UnitTests;

/// <summary>
/// Business Cycle
/// </summary>
internal sealed class SimpleCycle : Flow
{
    /// <summary>
    /// Static constructor to activate the initialization of the cycle.
    /// </summary>
    static SimpleCycle() => _ = new SimpleCycle();

    // Flow initializes these step fields through reflection.
    [Step(10)]
    public static Step Cycle_Started = null!;

    [Step(20)]
    public static Step Step_One = null!;

    [Step(30)]
    public static Step Step_Two = null!;

    [Step(40)]
    public static Step Step_Three = null!;

    [Step(50)]
    public static Step Step_Four = null!;

    [Step(60)]
    public static Step Step_Five = null!;

    [Step(70)]
    public static Step Cycle_Completed = null!;
}
