using Plugin.Constants.Formatting.Abstractions;

namespace EliteVA.Formats
{
    public class StatusFormat : IFormat
    {
        public string ToCommand(string name) => $"((EliteAPI.Status.{name}))";

        public string ToVariable(string name) => $"EliteAPI.{name}";
    }
}