namespace ColorSupport
{
    /// <summary>
    /// Determine if the terminal that runs your application support colors.
    /// </summary>
    public interface ITerminalSupport {
        /// <summary>
        /// Gets what the terminal's level of color support.
        /// </summary>
        SupportLevel Level { get; }
    }
}