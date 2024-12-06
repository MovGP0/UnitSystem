using System.Linq.Expressions;

namespace PassiveFlow;

public class Step(Flow flow)
{
    /// <summary>
    /// Name of the step.
    /// </summary>
    public string Name = "";

    /// <summary>
    /// Types associated to this step.
    /// </summary>
    public Type[]? AssociatedTypes = null;

    /// <summary>
    /// Step Id: is a number that explicitly unique to the step.
    /// </summary>
    public int Id = 0;


    /// <summary>
    /// Value of the step could be any arbitary thing.
    /// </summary>
    public object? Value = null;

    /// <summary>
    /// Indicates if this stage should be skipped during automatic transition of the flow host.
    /// </summary>
    public bool Skip = false;

    /// <summary>
    /// Describe the step
    /// </summary>
    public string Description_1 = "";

    /// <summary>
    /// Describe the stage
    /// </summary>
    public string Description_2 = "";


    /// <summary>
    /// The host type of this step
    /// </summary>
    public readonly Type FlowHostType = flow.GetType();

    /// <summary>
    /// the host object that host this step.
    /// </summary>
    public readonly Flow FlowContainer = flow;

    /// <summary>
    /// The actions of this step.
    /// Used when the step be an active step in the host.
    /// </summary>
    public StepAction[]? AttachedActions = null;

    public override string ToString()
    {
        return Name;
    }


    /// <summary>
    /// Get the id of the step.
    /// </summary>
    /// <param name="fi"></param>
    /// <returns></returns>
    public static implicit operator int(Step fi)
    {
        return fi.Id;
    }

    /// <summary>
    /// Get the name of the step.
    /// </summary>
    /// <param name="fi"></param>
    /// <returns></returns>
    public static implicit operator string(Step fi)
    {
        return fi.Name;
    }

    /// <summary>
    /// Get the index of this step in the host type.
    /// </summary>
    public int StepIndex
    {
        get
        {


            var Parent = CreateHostFlow();
            Parent.ReSetToStep(Id);
            return Parent.CurrentStepIndex;
        }
    }


    Func<Flow> HostFlowCreationFunction;

    private Flow CreateHostFlow()
    {
        //  (Flow)this.FlowHostType.Assembly.CreateInstance(this.FlowHostType.FullName)

        if (HostFlowCreationFunction == null)
        {
            var g = Expression.New(FlowHostType);

            var q = Expression.Lambda<Func<Flow>>(g, null);
            HostFlowCreationFunction =  q.Compile();
        }

        return HostFlowCreationFunction();

    }

    /// <summary>
    /// get the enclosing class and return new instance from it
    /// pointing to this item
    /// </summary>
    /// <param name="fi"></param>
    /// <returns></returns>
    public static implicit operator Flow(Step fi)
    {
        Flow Parent;
        if (fi.FlowHostType == typeof(Flow))
        {
            // dynamically created flow
            Parent = [];
            var fiContainer = fi.FlowContainer;
            foreach (var s in fiContainer)
            {
                var ns = Parent.Add(s.Name, s.Id, s.AssociatedTypes, s.Skip, s.AttachedActions);
                ns.Value = s.Value;
            }
        }
        else
        {
            Parent = (Flow)fi.CreateHostFlow();
        }
        Parent.ReSetToStep(fi.Id);
        return Parent;
    }
}