namespace PassiveFlow.Attributes;

/// <summary>
/// Attribute describe the stepping forward information.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class StepForwardActionAttribute : ActionAttribute
{
    /// <summary>
    /// Action to be known when one step back occure.
    /// </summary>
    /// <param name="actionId">Unique id of the action</param>
    /// <param name="actionText">Action description</param>
    /// <param name="statusId">Status of the action (to be known by the user logic)</param>
    public StepForwardActionAttribute(int actionId, string actionText, int statusId = 0)
        : base(actionId, actionText, 1, statusId)
    {
    }

    /// <summary>
    /// Action to be known when relative steps back from current step occure.
    /// </summary>
    /// <param name="actionId">Unique id of the action</param>
    /// <param name="actionText">Action description</param>
    /// <param name="relativeIndex">Positive number of the forward steps</param>
    /// <param name="statusId">Status of the action (to be known by the user logic)</param>
    public StepForwardActionAttribute(int actionId, string actionText, int relativeIndex, int statusId = 0)
        : base(actionId, actionText, (+1 * relativeIndex), statusId)
    {
    }
}