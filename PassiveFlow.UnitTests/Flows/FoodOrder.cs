namespace PassiveFlow.UnitTests;

internal sealed class FoodOrder : Flow
{
    /// <summary>
    /// Static constructor to activate the initialization of the cycle.
    /// </summary>
    static FoodOrder() => _ = new FoodOrder();

    // Flow initializes these step fields through reflection.
    [Step(10, typeof(HungryCustomer))]
    public static Step CustomerCall = null!;

    [Step(20, typeof(FoodCallCenter))]
    public static Step OrderWriting = null!;

    [Step(30, typeof(Kitchen))]
    public static Step OrderPreparation = null!;

    [Step(40, typeof(Kitchen))]
    public static Step OrderFinished = null!;

    [Step(50, typeof(DeliveryBoy))]
    public static Step OrderDelivery = null!;

    [Step(60, typeof(HungryCustomer))]
    public static Step OrderDelivered = null!;
}
