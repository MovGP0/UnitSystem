﻿using Microsoft.Scripting.Hosting.Shell;

public sealed class QsCommandLine : CommandLine
{
    protected override string Prompt => "Qs> ";

    protected override string ReadLine(int autoIndentSize)
    {
        Qs.Scripting.QsScriptCode.LastLine = base.ReadLine(autoIndentSize);

        if (!string.IsNullOrEmpty(Qs.Scripting.QsScriptCode.LastLine))
        {
            QsCommands.Engine = Engine;
            QsCommands.ScriptScope = ScriptScope;
            QsCommands.CheckCommand(Qs.Scripting.QsScriptCode.LastLine);

            if (QsCommands.CommandProcessed)
            {
                QsCommands.CommandProcessed = false;
                return string.Empty;
            }
        }

        return Qs.Scripting.QsScriptCode.LastLine;
    }

    protected override void UnhandledException(Exception e)
    {
        if (e is Qs.QsException qsException)
        {
            PrintError(qsException);
        }
        else
        {
            base.UnhandledException(e);
        }
    }

    public void PrintError(Exception e)
    {
        System.Console.ForegroundColor = QsCommands.ExceptionColor;
        Console.ErrorOutput.WriteLine("{0}: {1}", e.GetType().Name, e.Message);

        if (e.InnerException != null)
        {
            Console.ErrorOutput.WriteLine("{0}: {1}", e.InnerException.GetType().Name, e.InnerException.Message);
        }

        System.Console.ForegroundColor = QsCommands.ForegroundColor;
    }
}
