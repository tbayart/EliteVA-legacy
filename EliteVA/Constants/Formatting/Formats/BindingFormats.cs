namespace EliteVA.Constants.Formatting.Formats
{
    public class BindingFormat
    {
        public string ToVariable(string bindingName) => $"EliteAPI.{bindingName}";
    }
}