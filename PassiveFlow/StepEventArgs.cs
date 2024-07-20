namespace PassiveFlow;

public class StepEventArgs : EventArgs
{
    /// <summary>
    /// The step right before the current step.
    /// </summary>
    public Step PreviousStep { get; set; }

    /// <summary>
    /// The current step.
    /// </summary>
    public Step CurrentStep { get; set; }

    /// <summary>
    /// The action that were associated to this jump.
    /// </summary>
    public StepAction? StepAction { get; set; }

    public StepEventArgs(Step previousStep, Step currentStep, StepAction? stepAction)
    {
        PreviousStep = previousStep;
        CurrentStep = currentStep;
        StepAction = stepAction;
    }
}