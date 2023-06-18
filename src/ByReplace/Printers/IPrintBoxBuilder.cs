namespace ByReplace.Printers;

internal interface IPrintBox
{
    string BoxName { get; }
    int Width { get; }
    int Height { get; }

    string GetValuesToPrint();
}
