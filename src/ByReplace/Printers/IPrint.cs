namespace ByReplace.Printers;

public interface IPrint
{
    void Information(string text);
    void InformationTimer();
    void PrintWarning(string text);
    void PrintError(string text);
}
