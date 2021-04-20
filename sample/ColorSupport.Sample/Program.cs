using System;
using ColorSupport;

var support = new TerminalSupport();
var level = support.Level;

if (level == SupportLevel.None)
{
    Console.WriteLine("This is a plain text");
    return;
}

if (level == SupportLevel.Basic)
{
    var basicTag = "\u001b[{0}m{1}\u001b[0m";
    var ansiBasicBlue = 34;
    Console.WriteLine(
        string.Format(
            basicTag,
            ansiBasicBlue,
            "This is a text with basic colors"));
    return;
}

if (level == SupportLevel.Color256)
{
    var ansi256Tag = "\u001B[38;5;${0}m{1}\u001b[0m";
    var ansi256Orange = 214;
    Console.WriteLine(
        string.Format(
            ansi256Tag,
            ansi256Orange,
            "This is a text with up to 256 colors"));
    return;
}

if (level == SupportLevel.TrueColor)
{
    var rgbPattern = "\u001B[38;2;{0};{1};{2}m{3}\u001b[0m";
    int trueColorR = 153, trueColorG = 51, trueColorB = 102;
    Console.WriteLine(
        string.Format(
            rgbPattern,
            trueColorR,
            trueColorG,
            trueColorB,
            "This is a text with RGB colors"
        ));
}