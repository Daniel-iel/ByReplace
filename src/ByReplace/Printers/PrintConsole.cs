﻿namespace ByReplace.Printers;

internal class PrintConsole : IPrint
{
    readonly Stopwatch timer;
    readonly PrinterKonsole printerKonsole;

    public PrintConsole()
    {
        printerKonsole = new PrinterKonsole();
        timer = new Stopwatch();
        timer.Start();
    }

    private enum IPrintType
    {
        Info,
        Warning,
        Error
    };

    public void PrintInfo(string text)
    {
        printerKonsole.PrintColourText($"[{GetTimeElapsedText()} INF] {text}");
    }

    public void PrintWarning(string text)
    {
        printerKonsole.PrintColourText($"[{GetTimeElapsedText()} [Yellow]WRN] {text}");
    }

    public void PrintError(string text)
    {
        printerKonsole.PrintColourText($"[{GetTimeElapsedText()} [Red]ERR] {text}");
    }

    private string GetTimeElapsedText()
    {
        return $"{timer.Elapsed.ToString(@"mm\:ss\.fff")}";
    }
}