namespace UnitSystem.Gauge;

/// <summary>Provides a single-call entry point for checking gauge expressions.</summary>
public static class GaugeExpressionExtensions
{
    extension(GaugeExpression)
    {
        /// <summary>Parses and analyzes the supplied expression with the given field map or Standard Model defaults.</summary>
        public static GaugeProduct Analyze(string expression, IReadOnlyDictionary<string, GaugeRepresentation>? fields = null)
            => GaugeExpression.Parse(expression).Analyze(fields);
    }
}
