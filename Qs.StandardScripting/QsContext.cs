using System.Dynamic;
using Microsoft.Scripting;
using Microsoft.Scripting.Runtime;
using Microsoft.Scripting.Hosting;

namespace Qs.Scripting;

public sealed class QsContext : LanguageContext
{

    QsBinder _binder;

    public QsContext(ScriptDomainManager manager, IDictionary<string, object> options)
        : base(manager)
    {

        _binder = new QsBinder();  // I have defined end statement to close the execution of the program.
        // the end statement will return null which in return the execution of current program will end.
        // always write end in the final line of your qs program.

    }



    public override ConvertBinder CreateConvertBinder(Type toType, bool? explicitCast)
    {


        if (toType == typeof(int))
            return new QsIntegerConvertBinder();

        else
            return base.CreateConvertBinder(toType, explicitCast);

    }



    public override ScriptCode CompileSourceCode(SourceUnit sourceUnit, CompilerOptions options, ErrorSink errorSink)
    {

        var sc = new QsScriptCode(sourceUnit);

        return sc;
    }


    public override Version LanguageVersion
    {
        get
        {
            return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }
    }


    /// <summary>
    /// Creates the language runtime to be used in hosting.
    /// </summary>
    /// <returns></returns>
    public static ScriptRuntime CreateRuntime()
    {
        string[] QsNames = ["QuantitySystem", "Qs"];
        string[] QsExtensions = [".Qs"];
        var QsType = typeof(QsContext).FullName + ", " + typeof(QsContext).Assembly.FullName;

        var QsSetup = new LanguageSetup(QsType, "Quantity System Runtime", QsNames, QsExtensions);

        var srs = new ScriptRuntimeSetup();

        srs.LanguageSetups.Add(QsSetup);

        var sr = new ScriptRuntime(srs);

        return sr;
    }
}