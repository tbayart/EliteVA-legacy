using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;

namespace EliteVA.Services
{

    public class CommandService : ICommandService
    {
        private readonly ILogger<CommandService> log;
        private readonly IProxy proxy;
        public CommandService(ILogger<CommandService> log, IProxy proxy)
        {
            this.log = log;
            this.proxy = proxy;
        }
        
        public void InvokeCommand(string command)
        {
            if (proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult())
            {
                proxy.GetProxy().Commands.Invoke(command);      
            }
            else
            {
                log.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
            }
        }
    }
}