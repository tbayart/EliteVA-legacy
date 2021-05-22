using EliteVA.Constants.Formatting.Abstractions;

namespace EliteVA.Constants.Formatting.Formats
{
    public class StatusFormat
    {
        public string ToCommand(string statusName) => $"((EliteAPI.Status.{statusName}))";

        public string ToVariable(string statusName) => $"EliteAPI.{statusName}";
    }
}