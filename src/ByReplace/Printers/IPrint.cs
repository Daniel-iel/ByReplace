namespace ByReplace.Printers;

public interface IPrint
{
    void PureText(string text);
    void Information(string text);
    void Timer();
    void Warning(string text);
    void Error(string text);
    public void DrawBox(string boxName);
    public void Box(string text);
}
