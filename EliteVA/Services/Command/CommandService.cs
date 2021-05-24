using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;

namespace EliteVA.Services
{

    public class CommandService : ICommandService
    {
        private readonly ILogger<CommandService> log;
        private readonly IProxy proxy;
        private readonly IPaths paths;

        private readonly IList<(DateTime timestamp, string name, bool success)> invokedCommands;
        
        public CommandService(ILogger<CommandService> log, IProxy proxy, IPaths paths)
        {
            this.log = log;
            this.proxy = proxy;
            this.paths = paths;
            invokedCommands = new List<(DateTime timestamp, string name, bool success)>();
        }
        
        public void InvokeCommand(string command, bool skip)
        {
            var isSubbed = proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult();
            
            if (isSubbed)
            {
                invokedCommands.Add((DateTime.Now, command, true));
                proxy.GetProxy().Commands.Invoke(command);      
            }
            else
            {
                invokedCommands.Add((DateTime.Now, command, false));
                log.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
            }
            
            File.WriteAllLines(Path.Combine(paths.PluginDirectory.FullName, "InvokedCommands.txt"), invokedCommands.Select(x=> $"{x.timestamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} {x.name}"));
        }
    }
}