namespace PassiveFlow.Attributes;

/// <summary>
/// Describe the static step field in your class, give it unique identifier, and associted types.
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class StepAttribute : Attribute
{
    public int _StepId;
    public Type[] _ItemAssociatedTypes;
    public bool Skip = false;
    public string Descriptor_1 = string.Empty;
    public string Descriptor_2 = string.Empty;

    /// <summary>
    /// Identifier of the step.
    /// </summary>
    /// <param name="stepId">Unique integet id for the step</param>
    public StepAttribute(int stepId)
    {
        _StepId = stepId;
    }

    /// <summary>
    /// Identifier of the step beside the types that will be associated with this step.
    /// </summary>
    /// <param name="stepId">Unique integet id for the step</param>
    /// <param name="Identifier of the step"></param>
    public StepAttribute(int stepId, params Type[] associatedTypes)
    {
        _StepId = stepId;
        _ItemAssociatedTypes = associatedTypes;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stepId">Unique integet id for the step</param>
    public StepAttribute(int stepId, bool skipStep, params Type[] associatedTypes)
    {
        _StepId = stepId;
        _ItemAssociatedTypes = associatedTypes;
            
        Skip = skipStep;
    }
}