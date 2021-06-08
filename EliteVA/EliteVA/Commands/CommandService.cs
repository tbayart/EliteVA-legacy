using Microsoft.Extensions.Logging;
using Plugin.Constants.Paths.Abstractions;

namespace EliteVA.Commands
{
    public class CommandService : Plugin.Services.Command.CommandService
    {
        public CommandService(ILogger<Plugin.Services.Command.CommandService> log, IPaths paths) : base(log, paths)
        {
            
        }

        public override bool CommandExists(string command) => 
            VoiceAttackPlugin.Proxy.Commands.Exists(command).GetAwaiter().GetResult();

        public override void InvokeCommand(string command) =>
            VoiceAttackPlugin.Proxy.Commands.Invoke(command).GetAwaiter().GetResult();
    }
}