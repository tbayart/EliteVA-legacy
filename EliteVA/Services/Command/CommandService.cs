using EliteVA.Constants.Proxy.Abstractions;

namespace EliteVA.Services
{

    public class CommandService : ICommandService
    {
        private readonly IProxy proxy;
        public CommandService(IProxy proxy)
        {
            this.proxy = proxy;
        }
        
        public void InvokeCommand(string command)
        {
            if (proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult())
            {
                proxy.GetProxy().Commands.Invoke(command);      
            }
        }
    }
}