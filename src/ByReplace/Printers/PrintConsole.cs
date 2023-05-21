namespace ByReplace.Printers;

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

    public void Information(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} INF] {text}");
    }

    public void InformationTimer()
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} INF] [Green]{timer.Elapsed.Duration().ToString()}");
    }

    public void PrintWarning(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} [Yellow]WRN] {text}");
    }

    public void PrintError(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} [Red]ERR] {text}");
    }

    public void DrawBox(string boxName)
    {
        printerKonsole.DrawBox(boxName);
    }

    public void PrintToBox(string text)
    {
        printerKonsole.PrintToBox(text);
    }

    private string GetTimeElapsedText()
    {
        return $"{timer.Elapsed.ToString(@"mm\:ss\.fff")}";
    }
}