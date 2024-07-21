namespace PassiveFlow.UnitTests;

[TestClass]
public sealed class DynamicFlowTests
{
    [TestMethod]
    public void TestDynamicFlowOperations()
    {
        StringBuilder log = new();

        Flow dynamicFlow = [];
        dynamicFlow.StepChanged += Log;

        var f = dynamicFlow.Add("B1", 20);
        f.Value = "This is bad";
        f = dynamicFlow.Add("B2", 30);
        f = dynamicFlow.Add("B3", 40);
        f = dynamicFlow.Add("B4", 50);
        f = dynamicFlow.Add("F1", 80);
        f = dynamicFlow.Add("F2", 90);
        f.Value = 90;

        // Replace with expected output format
        dynamicFlow.ToString().ShouldBe("B1");

        dynamicFlow.StepLast();
        dynamicFlow.ToString().ShouldBe("F2");

        Flow df = dynamicFlow.CurrentStep;
        df.ToString().ShouldBe("F2");

        log.ToString().ShouldBe(Environment.NewLine);
        return;

        void Log(object? sender, StepEventArgs args)
            => log.AppendLine(args.StepAction?.ToString());
    }
}