[![CI/CD](https://github.com/R0tenur/color-support/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/R0tenur/color-support/actions/workflows/ci-cd.yml)
[![codecov](https://codecov.io/gh/R0tenur/color-support/branch/main/graph/badge.svg?token=0ET3K0PPZM)](https://codecov.io/gh/R0tenur/color-support)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Etimo/etimo-id/blob/master/LICENSE)
 ![Nuget](https://img.shields.io/nuget/dt/colorsupport)
# ColorSupport
Library to determine if the terminal that runs your application support colors.

## Installation
Package manager:
```sh
Install-Package ColorSupport -Version 1.0.0
```

Dotnet cli:
```sh
dotnet add package ColorSupport --version 1.0.0
```


## Usage
Getting the level:
```c#
ITerminalSupport support = new TerminalSupport();
SupportLevel level = support.Level;
```

Use it how ever you want: 
```c#
if (level == SupportLevel.None)
{
    Console.WriteLine("This is a plain text");
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
```
