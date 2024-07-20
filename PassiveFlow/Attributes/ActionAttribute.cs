namespace PassiveFlow.Attributes;

public abstract class ActionAttribute : Attribute
{
    public int ActionID { get; }
    public string ActionText { get; }
    public int RelativeIndex { get; }
    public int StageStatusID { get; }
    public string TargetStepName { get; }

    public ActionAttribute(int actionId, string actionText, int relativeIndex, int statusId = 0)
    {
        ActionID = actionId;
        ActionText = actionText;
        RelativeIndex = relativeIndex;
        StageStatusID = statusId;
    }

    public ActionAttribute(int actionId, string actionText, string targetStepName, int statusId = 0)
    {
        ActionID = actionId;
        ActionText = actionText;
        TargetStepName = targetStepName;
        StageStatusID = statusId;
    }
}