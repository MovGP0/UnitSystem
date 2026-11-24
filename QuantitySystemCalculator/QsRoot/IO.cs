using Qs.Runtime;
using Qs.Types;
using Qs;
using FluentFTP;
using System.Net;

namespace QsRoot;

/// <summary>
/// Standard I/O for quantity system.
/// </summary>
public static class Io
{
    private static readonly HttpClient DownloadClient = new(new SocketsHttpHandler
    {
        PooledConnectionLifetime = TimeSpan.FromMinutes(2),
        UseCookies = false
    });

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
        ArgumentNullException.ThrowIfNull(url);
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
        {
            return File.ReadAllText(url);
        }

        if (uri.IsFile)
        {
            return File.ReadAllText(uri.LocalPath);
        }

        if (uri.Scheme == Uri.UriSchemeFtp)
        {
            return DownloadFtpString(uri);
        }

        return DownloadClient.GetStringAsync(uri).GetAwaiter().GetResult();
    }

    private static string DownloadFtpString(Uri uri)
    {
        using var ftpClient = new FtpClient { Host = uri.IdnHost, Port = uri.Port };
        if (uri.UserInfo.Length > 0)
        {
            var credentials = uri.UserInfo.Split(':', 2);
            var user = Uri.UnescapeDataString(credentials[0]);
            var password = credentials.Length == 2 ? Uri.UnescapeDataString(credentials[1]) : string.Empty;
            ftpClient.Credentials = new NetworkCredential(user, password);
        }

        ftpClient.Connect();
        if (!ftpClient.DownloadBytes(out var bytes, Uri.UnescapeDataString(uri.AbsolutePath)))
        {
            throw new IOException("The FTP download did not complete.");
        }

        using var content = new MemoryStream(bytes);
        using var reader = new StreamReader(content);
        return reader.ReadToEnd();
    }

    internal static byte[] DownloadBytes(string url)
        => DownloadClient.GetByteArrayAsync(url).GetAwaiter().GetResult();

    public static string[] Files()
    {
        var di = new DirectoryInfo(Directory.GetCurrentDirectory());

        return di.GetFiles().Select(x => x.Name).ToArray();
    }
}
