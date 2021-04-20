namespace ColorSupport
{
    /// <summary>
    /// The level of what ansii colors that can be used in the current terminal.
    /// The different levels uses different escape codes.
    /// </summary>
    public enum SupportLevel
    {
        /// <summary>
        /// No colorsupport
        /// </summary>
        None = 0,
        /// <summary>
        /// Support for basic colors, 
        /// <para>can be used by Console.WriteLine($"\u001b[{colorNumber}m{myText}\u001b[0m")</para>
        ///<para>Wikipedia has a table for what numbers is possible to use: https://en.wikipedia.org/wiki/ANSI_escape_code#Colors</para>
        /// </summary>
        Basic = 1,

        /// <summary>
        /// Support for 256 colors, can be used by $"\u001B[38;5;${colorNumber}m{myText}\u001b[0m"
        ///<para>Wikipedia has a table for what numbers is possible to use: https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit</para>
        /// </summary>
        Color256 = 2,

        /// <summary>
        /// Support for 16 million colors (RGB), can be used by $"\u001B[38;2;{r};{g};{b}m{myText}\u001b[0m"
        /// </summary>
        TrueColor = 3,
    }
}