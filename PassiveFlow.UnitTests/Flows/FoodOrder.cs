namespace PassiveFlow.UnitTests;

#region Types involved in the food order
public class HungryCustomer;
public class FoodCallCenter;
public class Kitchen;
public class DeliveryBoy;
#endregion

public class FoodOrder : Flow
{
    /// <summary>
    /// Static constructor to activate the initialization of the cycle.
    /// </summary>
    static FoodOrder() => new FoodOrder();

    [Step(10, typeof(HungryCustomer))]
    public static Step CustomerCall;

    [Step(20, typeof(FoodCallCenter))]
    public static Step OrderWriting;

    [Step(30, typeof(Kitchen))]
    public static Step OrderPreparation;

    [Step(40, typeof(Kitchen))]
    public static Step OrderFinished;

    [Step(50, typeof(DeliveryBoy))]
    public static Step OrderDelivery;

    [Step(60, typeof(HungryCustomer))]
    public static Step OrderDelivered;
}