namespace Plugin.Constants.Formatting.Abstractions
{
    public interface IFormatting
    {
        /// <summary>
        /// Formats for events
        /// </summary>
        IFormat Events { get; }
        
        /// <summary>
        /// Formats for status
        /// </summary>
        IFormat Status { get; }
        
        /// <summary>
        /// Formats for bindings
        /// </summary>
        IFormat Bindings { get; }
        
        /// <summary>
        /// Formats for support
        /// </summary>
        IFormat Support { get; }
    }
}