namespace Plugin.Services.Command.Abstractions
{
    public interface ICommandService
    {
        /// <summary>
        /// Invokes a VoiceAttack command
        /// </summary>
        /// <param name="command">The name of the command</param>
        void InvokeIfExists(string command);
    }
}