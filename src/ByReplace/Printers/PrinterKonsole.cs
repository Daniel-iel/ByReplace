// Ignore Spelling: Konsole

using NoAlloq;

namespace ByReplace.Printers;

[ExcludeFromCodeCoverage]
internal sealed class PrinterKonsole
{
    readonly ConcurrentWriter console;
    IConsole consoleBox;

    public PrinterKonsole()
    {
        console = new ConcurrentWriter();
    }

    public void PrintColorText(string text)
    {
        ReadOnlySpan<string> textParts = new ReadOnlySpan<string>(text.Split(" "));

        bool shouldPrintColourText = textParts.Any(ShouldColourPredicate);
        if (!shouldPrintColourText)
        {
            Print(text);
            return;
        }

        foreach (ref readonly string textPart in textParts)
        {
            if (textPart.StartsWith("[Green]"))
            {
                console.Write(ConsoleColor.Green, $" {textPart.Replace("[Green]", "")}");
                continue;
            }

            if (textPart.StartsWith("[Yellow]"))
            {
                console.Write(ConsoleColor.Yellow, $" {textPart.Replace("[Yellow]", "")}");
                continue;
            }

            if (textPart.StartsWith("[Red]"))
            {
                console.Write(ConsoleColor.Red, $" {textPart.Replace("[Red]", "")}");
                continue;
            }

            if (textPart.StartsWith("[Cyan]"))
            {
                console.Write(ConsoleColor.Cyan, $" {textPart.Replace("[Cyan]", "")}");
                continue;
            }

            console.Write($" {textPart}");
        }

        console.WriteLine("");
    }

    public void Print(string text)
    {
        console.Write($" {text}");
        console.WriteLine("");
    }

    public void DrawBox(string boxName)
    {
        consoleBox = console.SplitLeft(boxName);
    }

    public void PrintToBox(string text)
    {
        consoleBox.WriteLine(text);
    }

    private bool ShouldColourPredicate(string text)
    {
        return text.StartsWith("[Green]", StringComparison.InvariantCultureIgnoreCase) ||
               text.StartsWith("[Yellow]", StringComparison.InvariantCultureIgnoreCase) ||
               text.StartsWith("[Red]", StringComparison.InvariantCultureIgnoreCase) ||
               text.StartsWith("[Cyan]", StringComparison.InvariantCultureIgnoreCase);
    }
}