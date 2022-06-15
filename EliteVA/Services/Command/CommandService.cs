using EliteAPI.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace EliteVA.Services
{

    public class CommandService : ICommandService
    {
        private readonly ILogger<CommandService> log;
        private readonly IProxy proxy;
        private readonly IPaths paths;
        private readonly IEliteDangerousApi api;
        private readonly IList<(DateTime timestamp, string name, bool success)> invokedCommands;

        public CommandService(ILogger<CommandService> log, IProxy proxy, IPaths paths, IEliteDangerousApi api)
        {
            this.log = log;
            this.proxy = proxy;
            this.paths = paths;
            this.api = api;
            invokedCommands = new List<(DateTime, string, bool)>();
        }

        public void InvokeCommand(string command)
        {
            if (api.HasCatchedUp == false)
            {
                log.LogDebug("Skipping '{Command}' during catchup", command);
                return;
            }

            if (proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult())
            {
                invokedCommands.Add((DateTime.Now, command, true));
                proxy.GetProxy().Commands.Invoke(command);
            }
            else
            {
                invokedCommands.Add((DateTime.Now, command, false));
                log.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
            }

            var commandsPath = Path.Combine(paths.PluginDirectory.FullName, "Commands");
            Directory.CreateDirectory(commandsPath);

            File.WriteAllLines(Path.Combine(commandsPath, "InvokedCommands.txt"), invokedCommands.Select(x => $"{x.timestamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} {x.name}"));
        }
    }
}
