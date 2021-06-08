using Plugin.Constants.Formatting.Abstractions;

namespace Plugin.Constants.Formatting
{
    public interface Formatting : IFormatting
    {
        IFormat Events { get; }

        IFormat Status { get; }
        
        IFormat Bindings { get; }

        IFormat Support { get; }
    }

}