using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Formatting.Formats;

namespace EliteVA.Constants.Formatting
{
    public class Formatting : IFormatting
    {
        public EventFormat Events => new EventFormat();

        public StatusFormat Status => new StatusFormat();
        
        public BindingFormat Bindings => new BindingFormat();
    }

}