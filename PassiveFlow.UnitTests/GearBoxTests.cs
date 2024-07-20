namespace PassiveFlow.UnitTests;

[TestClass]
public sealed class GearBoxTests
{
    [TestMethod]
    public void TestGearBoxOperationsAndLog()
    {
        // arrange
        var _log = new StringBuilder();
        Flow gearBox = GearBox.Neutral;

        // Register the event handler that writes to StringBuilder
        gearBox.StepChanged += (sender, e) => _log.AppendLine(e.StepAction?.ToString());

        // act

        gearBox++;
        gearBox++;
        gearBox++;
        gearBox++;
        gearBox--;
        gearBox--;
        gearBox--;
        gearBox--;
        gearBox += 2;
        gearBox--;
        gearBox--;
        gearBox++;
        gearBox++;

        gearBox.StepTo(GearBox.ReverseShift);

        gearBox += 4;
        gearBox--;
        gearBox--;
        gearBox--;
        gearBox--;

        var log = _log.ToString();

        gearBox++;
        gearBox++;
        gearBox++;
        gearBox++;

        var log2 = _log.ToString();

        // assert

        // Verify that the StringBuilder contains the expected log
        const string expectedLog = @"From Neutral To First Shift
From First Shift To SecondShift
From Second Shift To Third Shift
From Third Shift To Fourth Shift
From Fourth Shift To Third Shift
From Third Shift To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To First Shift
From First Shift To SecondShift

From Reverse To Third Shift
From Third Shift To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To Reverse Shift
";

        // Event handler should have logged each step
        const string expectedLog2 = @"From Neutral To First Shift
From First Shift To SecondShift
From Second Shift To Third Shift
From Third Shift To Fourth Shift
From Fourth Shift To Third Shift
From Third Shift To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To First Shift
From First Shift To SecondShift

From Reverse To Third Shift
From Third Shift To Second Shift
From Second Shift To First Shift
From First Shift To Neutral
From Neutral To Reverse Shift
From Reverse To Neutral
From Neutral To First Shift
From First Shift To SecondShift
From Second Shift To Third Shift
";

        "log".ShouldSatisfyAllConditions(
            () => log2.NormalizeLineEndings().ShouldBe(expectedLog2.NormalizeLineEndings()),
            () => log.NormalizeLineEndings().ShouldBe(expectedLog.NormalizeLineEndings()));
    }
}