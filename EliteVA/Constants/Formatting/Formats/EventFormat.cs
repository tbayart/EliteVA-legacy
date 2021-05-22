using EliteAPI.Event.Models.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;

namespace EliteVA.Constants.Formatting.Formats
{
    public class EventFormat
    {
        public string ToCommand(string eventName) => $"((EliteAPI.{eventName}))";
        
        public string ToCommand(IEvent e) => ToCommand(e.Event);
        
        public string ToVariable(string eventName, string name) => $"EliteAPI.{eventName}.{name}";
    }
}