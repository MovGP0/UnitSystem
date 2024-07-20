namespace PassiveFlow.UnitTests;

[TestClass]
public sealed class FoodOrderTests
{
    [TestMethod]
    public void TestCustomerStepsInFoodOrder()
    {
        StringBuilder log = new();
        Flow order = FoodOrder.CustomerCall;
        order.StepChanged += Log;

        Step[]? customerSteps = order.GetStepsOfType(typeof(HungryCustomer));

        // Replace with actual expected step names
        var expectedStepNames = new[]
        {
            "CustomerCall",
            "OrderDelivered"
        };

        var logEntries = log.ToString();

        customerSteps.ShouldSatisfyAllConditions(
            () => customerSteps?.Length.ShouldBe(expectedStepNames.Length),
            () => customerSteps?.Select(s => s.ToString()).ToArray().ShouldBe(expectedStepNames),
            () => logEntries.ShouldBe("")
        );

        return;

        void Log(object? sender, StepEventArgs args)
            => log.AppendLine(args.StepAction.ToString());
    }
}