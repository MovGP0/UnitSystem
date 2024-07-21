using Microsoft.Scripting.Hosting.Shell;

internal sealed class QsHost : ConsoleHost
{
    protected override Type Provider
        => typeof(Qs.Scripting.QsContext);

    protected override CommandLine CreateCommandLine()
        => new QsCommandLine();

    [STAThread]
    public static int Main(string[] args)
    {
        
        if (Environment.GetEnvironmentVariable("TERM") == null)
        {
            Environment.SetEnvironmentVariable("TERM", "dumb");
        }

        QsCommands.StartConsole();

        QuantitySystem.DynamicQuantitySystem.AddDynamicUnitConverterFunction("Currency", QsRoot.Currency.CurrencyConverter);

        return new QsHost().Run(args);
    }
}
