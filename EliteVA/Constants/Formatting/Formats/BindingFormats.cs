namespace EliteVA.Constants.Formatting.Formats
{
    public class BindingFormat
    {
        public string ToCommand(string name)
        {
            return name;
        }

        public string ToVariable(string bindingName) => $"EliteAPI.{bindingName}";
    }
}