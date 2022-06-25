using EliteVA.Constants.Formatting.Formats;

namespace EliteVA.Constants.Formatting.Abstractions
{
    public interface IFormatting
    {
        /// <summary>
        /// Formats for events
        /// </summary>
        EventFormat Events { get; }
        
        /// <summary>
        /// Formats for status
        /// </summary>
        StatusFormat Status { get; }
        
        /// <summary>
        /// Formats for bindings
        /// </summary>
        BindingFormat Bindings { get; }
        
        /// <summary>
        /// Formats for support
        /// </summary>
        SupportFormat Support { get; }
    }
}
