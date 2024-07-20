namespace PassiveFlow;

public class StepAction
{
    public string ActionText = string.Empty;   //text describing stage
    public int ActionId = int.MinValue;			//ID for for the text 
    public int TargetPosition = int.MinValue;		// target stage relative to this stage
    public string? TargetStepName = null;          // target flow item name
    public int ActionStatus = int.MinValue;		// integer used by host application to know status {approved,disapproved,waiting}
    // which have meaning in the host application

    public override string ToString() => ActionText;
}