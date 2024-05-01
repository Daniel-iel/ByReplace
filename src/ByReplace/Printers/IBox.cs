namespace ByReplace.Printers;

internal interface IBox
{
    string BoxName { get; }
    int Width { get; }
    int Height { get; }

    string GetValuesToPrint();
}
