using Plugin.Constants.Formatting.Abstractions;

namespace EliteVA.Formats
{
    public class SupportFormat : IFormat
    {
        public string ToCommand(string name) => $"((EliteAPI.{name}))";

        public string ToVariable(string name) => $"EliteAPI.{name}";
    }
}