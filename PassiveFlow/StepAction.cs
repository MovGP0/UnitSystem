namespace PassiveFlow;

public class StepAction
{
    /// <summary>
    /// text describing stage
    /// </summary>
    public string ActionText = string.Empty;

    /// <summary>
    /// ID for the text
    /// </summary>
    public int ActionId = int.MinValue;
    
    /// <summary>
    /// target stage relative to this stage
    /// </summary>
    public int TargetPosition = int.MinValue;
    
    /// <summary>
    /// target flow item name
    /// </summary>
    public string? TargetStepName = null;

    /// <summary>
    /// integer used by host application to know status {approved,disapproved,waiting}
    /// which have meaning in the host application
    /// </summary>
    public int ActionStatus = int.MinValue;

    public override string ToString() => ActionText;
}