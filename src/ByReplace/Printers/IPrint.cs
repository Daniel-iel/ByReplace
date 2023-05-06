namespace ByReplace.Printers;

public interface IPrint
{
    void PrintInfo(string text);
    void PrintWarning(string text);
    void PrintError(string text);
}
