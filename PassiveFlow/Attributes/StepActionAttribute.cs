namespace PassiveFlow.Attributes;

/// <summary>
/// Attribute describe the stepping information but based on the step name.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class StepActionAttribute : ActionAttribute
{
    /// <summary>
    /// Action to be known when one step back occure.
    /// </summary>
    /// <param name="actionId">Unique id of the action</param>
    /// <param name="actionText">Action description</param>
    /// <param name="statusId">Status of the action (to be known by the user logic)</param>
    /// <param name="targetStepName">Target step name {the static field in the class you are writign now (case sensitive)}</param>
    public StepActionAttribute(int actionId, string actionText, string targetStepName, int statusId = 0)
        : base(actionId, actionText, targetStepName, statusId)
    {
    }
}