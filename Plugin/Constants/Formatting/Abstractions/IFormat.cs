namespace Plugin.Constants.Formatting.Abstractions
{
    public interface IFormat
    {
        string ToCommand(string name);

        string ToVariable(string name);

    }
}