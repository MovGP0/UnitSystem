namespace PassiveFlow.UnitTests;

/// <summary>
/// Business Cycle
/// </summary>
public class SimpleCycle : Flow
{
    /// <summary>
    /// Static constructor to activate the initialization of the cycle.
    /// </summary>
    static SimpleCycle() => new SimpleCycle();

    [Step(10)]
    public static Step Cycle_Started;

    [Step(20)]
    public static Step Step_One;

    [Step(30)]
    public static Step Step_Two;

    [Step(40)]
    public static Step Step_Three;

    [Step(50)]
    public static Step Step_Four;

    [Step(60)]
    public static Step Step_Five;

    [Step(70)]
    public static Step Cycle_Completed;
}