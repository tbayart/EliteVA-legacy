namespace EliteVA.Services
{
    public interface ICommandService
    {
        /// <summary>
        /// Invokes a VoiceAttack command
        /// </summary>
        /// <param name="command">The name of the command</param>
        void InvokeCommand(string command);
    }
}