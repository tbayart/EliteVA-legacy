namespace EliteVA.Constants.Formatting.Formats
{
    public class SupportFormat
    {
        public string ToCommand(string type) => $"((EliteAPI.{type}))";

        public string ToVariable(string type) => $"EliteAPI.{type}";
    }
}