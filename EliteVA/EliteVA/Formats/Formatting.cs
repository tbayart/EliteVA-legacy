using Plugin.Constants.Formatting.Abstractions;

namespace EliteVA.Formats
{
    public class Format : IFormatting
    {
        public IFormat Events => new EventFormat();
        public IFormat Status => new StatusFormat();
        
        public IFormat Bindings => new BindingFormat();
        public IFormat Support => new SupportFormat();
    }
}