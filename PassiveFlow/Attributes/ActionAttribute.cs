namespace PassiveFlow.Attributes;

public abstract class ActionAttribute(int actionId, string actionText, int relativeIndex, int statusId = 0)
    : Attribute
{
    public int ActionID { get; } = actionId;
    public string ActionText { get; } = actionText;
    public int RelativeIndex { get; } = relativeIndex;
    public int StageStatusID { get; } = statusId;
    public string TargetStepName { get; }

    public ActionAttribute(int actionId, string actionText, string targetStepName, int statusId = 0) : this(actionId, actionText, 0, statusId)
    {
        TargetStepName = targetStepName;
    }
}