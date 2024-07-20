namespace PassiveFlow.UnitTests;

public class GearBox : Flow
{
    /// <summary>
    /// Static constructor to activate the initialization of the cycle.
    /// </summary>
    static GearBox()
    {
        new GearBox();
    }

    [StepAction(-15, "From Reverse To Fifth Shift", "FifthShift")]
    [StepAction(-14, "From Reverse To Fourth Shift", "FourthShift")]
    [StepAction(-13, "From Reverse To Third Shift", "ThirdShift")]
    [StepAction(-12, "From Reverse To Second Shift", "SecondShift")]
    [StepAction(-11, "From Reverse To First Shift", "FirstShift")]
    [StepAction(-10, "From Reverse To Neutral", "Neutral")]
    [Step(-1)]
    public static Step ReverseShift;

    [StepAction(03, "From Neutral To Third Shift", "ThirdShift")]
    [StepAction(02, "From Neutral To Second Shift", "SecondShift")]
    [StepAction(01, "From Neutral To First Shift", "FirstShift")]
    [StepSelfAction(0, "Neutral Transmition")]
    [StepAction(0-1, "From Neutral To Reverse Shift", "ReverseShift")]
    [Step(0)]
    public static Step Neutral;


    [StepAction(10, "From First Shift To Neutral", "Neutral")]
    [StepAction(12, "From First Shift To SecondShift", "SecondShift")]
    [Step(1)]
    public static Step FirstShift;

    [StepAction(21, "From Second Shift To First Shift", "FirstShift")]
    [StepAction(23, "From Second Shift To Third Shift", "ThirdShift")]
    [Step(2)]
    public static Step SecondShift;

    [StepAction(32, "From Third Shift To Second Shift", "SecondShift")]
    [StepAction(34, "From Third Shift To Fourth Shift", "FourthShift")]
    [Step(3)]
    public static Step ThirdShift;

    [StepAction(43, "From Fourth Shift To Third Shift", "ThirdShift")]
    [StepAction(45, "From Fourth Shift To Fifth Shift", "FifthShift")]
    [Step(4)]
    public static Step FourthShift;

    [StepAction(54, "From Fifth Shift To Fourth Shift", "FourthShift")]
    [Step(5)]
    public static Step FifthShift;
}
