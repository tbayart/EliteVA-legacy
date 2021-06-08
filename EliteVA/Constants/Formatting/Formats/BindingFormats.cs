using System.Runtime.Serialization;
using EliteVA.Constants.Formatting.Abstractions;

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