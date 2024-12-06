namespace PassiveFlow.Attributes;

/// <summary>
/// Decorating the step if the step jumped to itself.
/// </summary>
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class StepSelfActionAttribute(int actionId, string actionText, int statusId = 0)
    : StepForwardActionAttribute(actionId, actionText, 0, statusId);