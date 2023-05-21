﻿namespace ByReplace.Printers;

internal class PrintBox
{
    private IConsole consoleBox;

    public void CreateBox(string boxName)
    {
        CreateBox(boxName, 100, 15);
    }

    public void CreateBox(string boxName, int with)
    {
        CreateBox(boxName, with, 15);
    }

    public void CreateBox(string boxName, int with, int height)
    {
        consoleBox = Window.OpenBox(boxName, with, height);
    }

    public void Print(string text)
    {
        consoleBox.WriteLine(text);
    }
}