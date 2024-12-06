namespace PassiveFlow;

public class StepEventArgs(Step previousStep, Step currentStep, StepAction? stepAction)
    : EventArgs
{
    /// <summary>
    /// The step right before the current step.
    /// </summary>
    public Step PreviousStep { get; set; } = previousStep;

    /// <summary>
    /// The current step.
    /// </summary>
    public Step CurrentStep { get; set; } = currentStep;

    /// <summary>
    /// The action that were associated to this jump.
    /// </summary>
    public StepAction? StepAction { get; set; } = stepAction;
}