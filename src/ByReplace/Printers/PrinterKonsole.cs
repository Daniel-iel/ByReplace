using Konsole;
using NoAlloq;

namespace ByReplace.Printers;

internal class PrinterKonsole
{
    readonly ConcurrentWriter console;

    public PrinterKonsole()
    {
        console = new ConcurrentWriter();
    }

    public void PrintColourText(string text)
    {
        ReadOnlySpan<string> textParts = new ReadOnlySpan<string>(text.Split(" "));

        var shouldPrintColourText = textParts.Any(ShouldColourPredicate); //.Any(ShouldColourPredicate);
        if (!shouldPrintColourText)
        {
            Print(text);
            return;
        }

        foreach (var textPart in textParts)
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

            console.Write($" {textPart}");
        }

        console.WriteLine("");
    }

    public void Print(string text)
    {
        console.Write($" {text}");
        console.WriteLine("");
    }

    private bool ShouldColourPredicate(string text)
    {
        return text.StartsWith("[Green]", StringComparison.InvariantCultureIgnoreCase) ||
               text.StartsWith("[Yellow]", StringComparison.InvariantCultureIgnoreCase) ||
               text.StartsWith("[Red]", StringComparison.InvariantCultureIgnoreCase);
    }
}