namespace UnitSystem.Quantities;

/// <summary>
/// Provides extension methods for performing arithmetic operations on generic types.
/// </summary>
internal static class OperatorExtensions
{
    /// <summary>
    /// Adds two values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>The result of adding <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static T Add<T>(this T a, T b) where T : struct => (dynamic)a + (dynamic)b;

    /// <summary>
    /// Subtracts the second value from the first value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>The result of subtracting <paramref name="b"/> from <paramref name="a"/>.</returns>
    public static T Subtract<T>(this T a, T b) where T : struct => (dynamic)a - (dynamic)b;

    /// <summary>
    /// Multiplies two values of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>The result of multiplying <paramref name="a"/> and <paramref name="b"/>.</returns>
    public static T Multiply<T>(this T a, T b) where T : struct => (dynamic)a * (dynamic)b;

    /// <summary>
    /// Multiplies a value of type <typeparamref name="T"/> by a double value.
    /// </summary>
    /// <typeparam name="T">The type of the first value, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The double value.</param>
    /// <returns>The result of multiplying <paramref name="a"/> by <paramref name="b"/>.</returns>
    public static T Multiply<T>(this T a, double b) where T : struct => (dynamic)a * b;

    /// <summary>
    /// Divides the first value by the second value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the values, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The second value.</param>
    /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
    public static T Divide<T>(this T a, T b) where T : struct => (dynamic)a / (dynamic)b;

    /// <summary>
    /// Divides a value of type <typeparamref name="T"/> by a double value.
    /// </summary>
    /// <typeparam name="T">The type of the first value, which must be a struct.</typeparam>
    /// <param name="a">The first value.</param>
    /// <param name="b">The double value.</param>
    /// <returns>The result of dividing <paramref name="a"/> by <paramref name="b"/>.</returns>
    public static T Divide<T>(this T a, double b) where T : struct => (dynamic)a / b;

    /// <summary>
    /// Raises a value of type <typeparamref name="T"/> to the power of an integer exponent.
    /// </summary>
    /// <typeparam name="T">The type of the value, which must be a struct.</typeparam>
    /// <param name="a">The base value.</param>
    /// <param name="exponent">The exponent.</param>
    /// <returns>The result of raising <paramref name="a"/> to the power of <paramref name="exponent"/>.</returns>
    public static T Pow<T>(this T a, int exponent) where T : struct => (T)Math.Pow((dynamic)a, exponent);

    /// <summary>
    /// Raises a value of type <typeparamref name="T"/> to the power of a double exponent.
    /// </summary>
    /// <typeparam name="T">The type of the value, which must be a struct.</typeparam>
    /// <param name="a">The base value.</param>
    /// <param name="exponent">The exponent.</param>
    /// <returns>The result of raising <paramref name="a"/> to the power of <paramref name="exponent"/>.</returns>
    public static T Pow<T>(this T a, double exponent) where T : struct => (T)Math.Pow((dynamic)a, exponent);
}
