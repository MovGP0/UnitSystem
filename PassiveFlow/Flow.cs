using PassiveFlow.Attributes;
using System.Reflection;

namespace PassiveFlow;

public class Flow : IEnumerable<Step>
{
        
    public event EventHandler<StepEventArgs>? StepChanged;

    //holding instance of every Step Field
    private List<Step> innerSteps = new List<Step>();

    //holding the field types
    private List<FieldInfo> _MyFields = new  List<FieldInfo>();

    /// <summary>
    /// this constructor initialize the class that is inherited from the Flow class to know the fields that marked as steps
    /// </summary>
    public Flow()
    {
        //getting the type of this instance
        Type FlowType = GetType();

        FieldInfo[] FlowFields = FlowType.GetFields();
        foreach (FieldInfo fi in FlowFields)
        {
            StepAttribute[] fia = (StepAttribute[])fi.GetCustomAttributes(typeof(StepAttribute), true);
            if (fia.Length > 0)
            {
                //then this field is a flow step
                _MyFields.Add(fi);//caching the field info

                //get all of its action
                ActionAttribute[] aas = (ActionAttribute[])fi.GetCustomAttributes(typeof(ActionAttribute), true);
                Step fli;
                if (aas.Length > 0)
                {
                    StepAction[] acts = new StepAction[aas.Length];
                    for (int i = 0; i < aas.Length; i++)
                    {
                        StepAction aa = new StepAction();
                        aa.ActionId = aas[i].ActionID;
                        aa.ActionText = aas[i].ActionText;
                        if (aas[i].GetType().Equals(typeof(StepActionAttribute)))
                            aa.TargetStepName = aas[i].TargetStepName;
                        else
                            aa.TargetPosition = aas[i].RelativeIndex;
                        aa.ActionStatus = aas[i].StageStatusID;
                        acts[i] = aa;
                    }
                    fli = Add(fi.Name, fia[0]._StepId, fia[0]._ItemAssociatedTypes, fia[0].Skip, acts);
                }
                else
                {
                    fli = Add(fi.Name, fia[0]._StepId, fia[0]._ItemAssociatedTypes, fia[0].Skip);
                }

                fli.Description_1 = fi.Name;
                if (fia[0].Descriptor_1 != "") fli.Description_1 = fia[0].Descriptor_1;
                fli.Description_2 = fia[0].Descriptor_2;
                innerSteps[innerSteps.Count - 1] = fli;

                fi.SetValue(this, fli);
            }
        }
    }

    public FieldInfo[] MyFields
    {
        get
        {
            return _MyFields.ToArray();
        }
    }


    #region Flowstep Add procedures
    public Step Add(string stepName, int stepId)
    {
        Step fi = new Step(this);
        fi.Name = stepName;
        fi.Id = stepId;
        innerSteps.Add(fi);
        return fi;
    }

    public Step Add(string stepName, int stepId, params Type[] stepAssociatedTypes)
    {
        Step fi = new Step(this);
        fi.Name = stepName;
        fi.Id = stepId;
        fi.AssociatedTypes = stepAssociatedTypes;
        innerSteps.Add(fi);
        return fi;
    }

    private Step Add(string stepName, int stepId, Type[] stepAssociatedTypes, bool skip)
    {
        Step fi = new Step(this);
        fi.Name = stepName;
        fi.Id = stepId;
        fi.AssociatedTypes = stepAssociatedTypes;
            
        fi.Skip = skip;

        innerSteps.Add(fi);
        return fi;
    }

    public Step Add(string stepName, int stepId, Type[] stepAssociatedTypes, bool skip, params StepAction[] stepActions)
    {
        Step fi = new Step(this);
        fi.Name = stepName;
        fi.Id = stepId;
        fi.AssociatedTypes = stepAssociatedTypes;
            
        fi.Skip = skip;
        fi.AttachedActions = stepActions;

        innerSteps.Add(fi);
        return fi;
    }


    #endregion



    /// <summary>
    /// fast action retrival
    /// </summary>
    /// <param name="actionId"></param>
    /// <returns></returns>
    public string GetActionDescription(int actionId)
    {
        foreach (Step fi in this)
        {
            if (fi.AttachedActions != null)
            {
                foreach (StepAction act in fi.AttachedActions)
                {
                    if (act.ActionId == actionId)
                    {
                        return act.ActionText;
                    }
                }
            }
        }
        return "";
    }

    /// <summary>
    /// fast stage name retrival
    /// </summary>
    /// <param name="stepId"></param>
    /// <returns></returns>
    public string GetStepName(int stepId)
    {
        foreach (Step fi in this)
        {
            if (fi.Id == stepId)
                return fi.Name;
        }
        return "";
    }




    public Step FirstStep
    {
        get
        {
            return FlowSteps[FlowSteps.GetLowerBound(0)];
        }
    }

    public Step LastStep
    {
        get
        {
            return FlowSteps[FlowSteps.GetUpperBound(0)];
        }
    }
    public Step GetFlowStep(int index)
    {
        return FlowSteps[index];
    }

    /// <summary>
    /// returns all steps that have this target Type associated
    /// with it else return null
    /// </summary>
    /// <param name="associatedType"></param>
    /// <returns></returns>
    public Step[]? GetStepsOfType(Type associatedType)
    {
        List<Step> al = new  List<Step>();
        foreach (Step fli in this)
        {
            if (fli.AssociatedTypes != null)
            {
                foreach (Type tp in fli.AssociatedTypes)
                {
                    if (tp == associatedType || associatedType.IsSubclassOf(tp))
                        al.Add(fli);
                }
            }
        }
        if (al.Count > 0) return al.ToArray();
        else
            return null;
    }

    public Step this[int index]
    {
        get
        {
            foreach (Step fi in innerSteps)
            {
                if (fi.Id == index) return fi;
            }
            throw new StepException("Flow step not found");
        }
    }

    public Step this[string name]
    {
        get
        {
            foreach (Step fi in innerSteps)
            {
                if (fi.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return fi;

            }

            throw new StepException("Flow step not found");
        }
    }

    public Step[] FlowSteps
    {
        get
        {
            return (Step[])innerSteps.ToArray();
        }
    }


    protected int _Previous_StepIndex;
    protected int _Current_StepIndex;

    public int CurrentStepIndex
    {
        get { return _Current_StepIndex; }
    }

    #region Flowstep Pointer Control

    /// <summary>
    /// Set the current step in the top of the flow
    /// </summary>
    public void StepFirst()
    {
        _Previous_StepIndex = _Current_StepIndex;

        _Current_StepIndex = FlowSteps.GetLowerBound(0);

        if (StepChanged != null) StepChanged(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));
    }


    /// <summary>
    /// Go forward one step.
    /// </summary>
    public void StepForward()
    {
        StepForward(1);
    }

    /// <summary>
    /// Go forward for a number of steps.
    /// </summary>
    /// <param name="steps"></param>
    public void StepForward(int steps)
    {
        _Previous_StepIndex = _Current_StepIndex;

        skipping:
        if (_Current_StepIndex + steps <= FlowSteps.GetUpperBound(0))
            _Current_StepIndex += steps;

        // retry the sub if current step should be eleminated from the 
        // current cycle
        if (CurrentStep.Skip && CurrentStep.Id != LastStep.Id)
        {
            steps = 1;
            goto skipping;
        }

        if (StepChanged != null) StepChanged(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));

    }

    public void StepBack()
    {
        StepBack(1);
    }

    public void StepBack(int steps)
    {
        _Previous_StepIndex = _Current_StepIndex;

        skipping:
        if (_Current_StepIndex - steps >= FlowSteps.GetLowerBound(0))
            _Current_StepIndex -= steps;

        // retry the sub if current step should be eleminated from the 
        // current cycle
        if (CurrentStep.Skip && CurrentStep.Id != FirstStep.Id)
        {
            steps = 1;
            goto skipping;
        }

        if (StepChanged != null) StepChanged(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));
    }


    /// <summary>
    /// go to the last step 
    /// </summary>
    public void StepLast()
    {
        _Previous_StepIndex = _Current_StepIndex;
        _Current_StepIndex = FlowSteps.GetUpperBound(0);

        StepChanged?.Invoke(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));
    }

    /// <summary>
    /// goto will set active step to the passed index
    /// even if it is defined to be skipped
    /// </summary>
    /// <param name="stepIndex"></param>
    public void Goto(int stepIndex)
    {
        if (stepIndex <= FlowSteps.GetUpperBound(0))
        {
            _Previous_StepIndex = _Current_StepIndex;

            _Current_StepIndex = stepIndex;

            if (StepChanged != null) StepChanged(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));
        }
    }


    /// <summary>
    /// Step to the desired identified step.
    /// </summary>
    /// <param name="stepId"></param>
    public void StepTo(int stepId)
    {
        _Previous_StepIndex = _Current_StepIndex;

        int i = FlowSteps.GetLowerBound(0);
        foreach (Step fi in innerSteps)
        {
            if (fi.Id == stepId)
            {
                _Current_StepIndex = i;
                break;
            }
            i++;
        }

        if (StepChanged != null) StepChanged(this, new StepEventArgs(PreviousStep, CurrentStep, ActiveAction));
    }


    /// <summary>
    /// ReSet the current step to the passed step id
    /// even if the desired step marked to be skipped.
    /// </summary>
    /// <param name="stepId"></param>
    public void ReSetToStep(int stepId)
    {
        _Previous_StepIndex = _Current_StepIndex;

        int i = FlowSteps.GetLowerBound(0);
        foreach (Step fi in innerSteps)
        {
            if (fi.Id == stepId)
            {
                _Current_StepIndex = i;
                _Previous_StepIndex = i;
                break;
            }
            i++;
        }

    }


    #endregion


    /// <summary>
    /// The step that was before the current active step.
    /// </summary>
    public Step PreviousStep
    {
        get
        {
            return FlowSteps[_Previous_StepIndex];
        }
    }


    /// <summary>
    /// Returns the current step that the flow is waiting at.
    /// </summary>
    public Step CurrentStep
    {
        get
        {
            return FlowSteps[_Current_StepIndex];
        }
    }


    /// <summary>
    /// Advance the flow forward step.
    /// </summary>
    /// <param name="fl"></param>
    /// <returns></returns>
    public static Flow operator ++(Flow fl)
    {
        fl.StepForward();
        return fl;
    }

    public static Flow operator +(Flow flow, int steps)
    {
        flow.StepForward(steps);
        return flow;
    }

    /// <summary>
    /// Advance the flow previous step.
    /// </summary>
    /// <param name="fl"></param>
    /// <returns></returns>
    public static Flow operator --(Flow fl)
    {
        fl.StepBack();
        return fl;
    }

    public static Flow operator -(Flow flow, int steps)
    {
        flow.StepBack(steps);
        return flow;
    }

    public static implicit operator string(Flow fl)
    {
        return fl.CurrentStep.Name;
    }

    public override string ToString()
    {
        return CurrentStep.Name;
    }

    public static explicit operator int(Flow fl)
    {
        return fl.CurrentStep.Id;
    }

    public static implicit operator Step(Flow fl)
    {
        return fl.CurrentStep;
    }

    #region comparison operators
    public static bool operator ==(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.Id == fl2.CurrentStep.Id)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public override bool Equals(object obj)
    {
        Flow fl2 = (Flow)obj;
        if (GetType().Equals(fl2.GetType()))
        {
            if (CurrentStep.Id == fl2.CurrentStep.Id)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }

    }

    public static bool operator !=(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.Id != fl2.CurrentStep.Id)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public static bool operator >(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.StepIndex > fl2.CurrentStep.StepIndex)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public static bool operator <(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.StepIndex < fl2.CurrentStep.StepIndex)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public static bool operator >=(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.StepIndex >= fl2.CurrentStep.StepIndex)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }

    public static bool operator <=(Flow fl1, Flow fl2)
    {
        if (fl1.GetType().Equals(fl2.GetType()))
        {
            if (fl1.CurrentStep.StepIndex <= fl2.CurrentStep.StepIndex)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
    }


    #endregion


    /// <summary>
    /// get the latest action 
    /// in relation between active_step and last_active_step
    /// otherwise return null
    /// </summary>
    public StepAction? ActiveAction
    {
        get
        {
            int delta = _Current_StepIndex - _Previous_StepIndex;

            //search in last active step actions
            //with position that is equal to delta
            if (PreviousStep.AttachedActions != null)
            {
                foreach (StepAction flact in PreviousStep.AttachedActions)
                {
                    if (flact.TargetStepName != null)
                    {
                        Step RequiredFlowStep = this[flact.TargetStepName];
                        if (CurrentStep.Id == RequiredFlowStep.Id)
                            return flact;
                    }
                    else
                    if (flact.TargetPosition == delta)
                    {
                        return flact;
                    }
                }
            }
            return null;
        }
    }

    public IEnumerator<Step> GetEnumerator()
    {
        return innerSteps.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return innerSteps.GetEnumerator();
    }
}