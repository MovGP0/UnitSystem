using System.Reflection;
using Pastel;
using SymbolicAlgebra;
using SymbolicAlgebra.Console;

const string Prompt = "SAC> ";

string line = string.Empty;
Type ts = typeof(SymbolicVariable);

var assemblyVersion = ts.Assembly
    .GetCustomAttributes<AssemblyFileVersionAttribute>()
    .First()
    .Version;

string copyright = string.Format(Resources.Copyright, assemblyVersion, DateTime.Now.Year);

Console.WriteLine(copyright.Pastel(ConsoleColor.DarkYellow));
Console.WriteLine();
Console.WriteLine(Resources.Help.Pastel(ConsoleColor.DarkYellow));
Console.WriteLine();
Console.WriteLine(Resources.Help2.Pastel(ConsoleColor.DarkYellow));

while (line.ToUpperInvariant() != ":Q")
{
    Console.Write(Prompt.Pastel(ConsoleColor.White));
    line = Console.ReadLine();

    if (!string.IsNullOrEmpty(line))
    {
        if (line.ToUpperInvariant() == ":Q") break;

        try
        {
            var result = SymbolicVariable.Parse(line);
            Console.WriteLine(("     " + result).Pastel(ConsoleColor.Yellow));
        }
        catch(SymbolicException se)
        {
            Console.WriteLine(se.Message.Pastel(ConsoleColor.Magenta));
        }
    }
    else
    {
        line = string.Empty;
    }
}