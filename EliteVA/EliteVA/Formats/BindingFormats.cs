using Plugin.Constants.Formatting.Abstractions;

namespace EliteVA.Formats
{
    public class BindingFormat : IFormat
    {
        public string ToCommand(string name) => name;

        public string ToVariable(string name) => $"EliteAPI.{name}";
    }
}