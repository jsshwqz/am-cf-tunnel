using System.Diagnostics;

namespace Screencast.Client;

internal static class Program
{
    private static int Main(string[] args)
    {
        Console.WriteLine("Android Screencast (Windows EXE) - Bootstrap");
        Console.WriteLine("当前为实现阶段的占位版本，用于验证构建流程与基础能力。");

        var command = args.Length > 0 ? args[0] : "help";
        return command switch
        {
            "doctor" => RunDoctor(),
            "help" => ShowHelp(),
            _ => ShowHelp()
        };
    }

    private static int ShowHelp()
    {
        Console.WriteLine("用法:");
        Console.WriteLine("  Screencast.Client.exe doctor  # 检查 ADB 是否可用");
        return 0;
    }

    private static int RunDoctor()
    {
        var adbPath = AdbLocator.FindAdb();
        if (adbPath is null)
        {
            Console.WriteLine("未检测到 adb，请确保已安装 Android Platform Tools 并加入 PATH。");
            return 1;
        }

        Console.WriteLine($"检测到 adb: {adbPath}");
        var result = AdbRunner.Run(adbPath, "devices");
        Console.WriteLine(result.StandardOutput);
        if (!string.IsNullOrWhiteSpace(result.StandardError))
        {
            Console.WriteLine("错误输出:");
            Console.WriteLine(result.StandardError);
        }

        return result.ExitCode;
    }
}

internal static class AdbLocator
{
    public static string? FindAdb()
    {
        var adbFromPath = FindOnPath("adb.exe");
        if (adbFromPath is not null)
        {
            return adbFromPath;
        }

        var localSdk = Environment.GetEnvironmentVariable("ANDROID_HOME")
            ?? Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
        if (!string.IsNullOrWhiteSpace(localSdk))
        {
            var candidate = Path.Combine(localSdk, "platform-tools", "adb.exe");
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static string? FindOnPath(string fileName)
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(path))
        {
            return null;
        }

        foreach (var segment in path.Split(Path.PathSeparator))
        {
            if (string.IsNullOrWhiteSpace(segment))
            {
                continue;
            }

            var candidate = Path.Combine(segment.Trim(), fileName);
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }
}

internal sealed record AdbResult(int ExitCode, string StandardOutput, string StandardError);

internal static class AdbRunner
{
    public static AdbResult Run(string adbPath, string arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = adbPath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(startInfo);
        if (process is null)
        {
            return new AdbResult(1, string.Empty, "无法启动 adb 进程。");
        }

        var stdout = process.StandardOutput.ReadToEnd();
        var stderr = process.StandardError.ReadToEnd();
        process.WaitForExit();

        return new AdbResult(process.ExitCode, stdout, stderr);
    }
}
