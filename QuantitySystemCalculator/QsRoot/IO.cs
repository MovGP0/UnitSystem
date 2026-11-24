using Qs.Runtime;
using Qs.Types;
using Qs;
using System.Net;

namespace QsRoot;

/// <summary>
/// Standard I/O for quantity system.
/// </summary>
public static class Io
{
    public static QsValue InputLine(string prompt)
    {
        Console.Write(prompt);
        return InputLine();
    }

    public static QsValue InputLine()
    {
        return new QsText(Console.ReadLine() ?? throw new EndOfStreamException("No more console input is available."));
    }

    public static QsValue? PrintLine([QsParamInfo(QsParamType.Text)] QsParameter val)
    {
        Console.WriteLine(val.UnknownValueText);
        return null;
    }

    public static QsValue ReadBinaryFile([QsParamInfo(QsParamType.Text)]QsParameter fileName)
    {
        var data = File.ReadAllBytes(fileName.UnknownValueText ?? throw new QsException("A file name is required."));

        return data.ToQsVector();
    }

    public static QsValue? Speak(
        [QsParamInfo(QsParamType.Text)] QsParameter text
    )
    {
        var ss = new System.Speech.Synthesis.SpeechSynthesizer();

        ss.SpeakAsync(text.UnknownValueText ?? throw new QsException("Text to speak is required."));

        return null;
    }

    public static string DownloadString(string url)
    {
        var result = string.Empty;

        using (var client = new WebClient()) // WebClient class inherits IDisposable
        {
            result = client.DownloadString(url);
        }

        return result;
    }

    public static string[] Files()
    {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory());

        return di.GetFiles().Select(x => x.Name).ToArray();
    }
}
