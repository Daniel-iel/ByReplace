namespace ByReplace.Printers;

[ExcludeFromCodeCoverage]
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
    public void PureText(string text)
    {
        printerKonsole.PrintColorText($"{text}");
    }

    public void Information(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} INF] {text}");
    }

    public void Timer()
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} INF] [Green]{timer.Elapsed.Duration().ToString()}");
    }

    public void Warning(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} [Yellow]WRN] {text}");
    }

    public void Error(string text)
    {
        printerKonsole.PrintColorText($"[{GetTimeElapsedText()} [Red]ERR] {text}");
    }

    public void DrawBox(string boxName)
    {
        printerKonsole.DrawBox(boxName);
    }

    public void Box(string text)
    {
        printerKonsole.PrintToBox(text);
    }

    private string GetTimeElapsedText()
    {
        return $"{timer.Elapsed.ToString(@"mm\:ss\.fff")}";
    }
}