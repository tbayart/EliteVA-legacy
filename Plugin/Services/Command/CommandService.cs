using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Plugin.Constants.Paths.Abstractions;
using Plugin.Services.Command.Abstractions;

namespace Plugin.Services.Command
{

    public abstract class CommandService : ICommandService
    {
        private readonly ILogger<CommandService> log;
        private readonly IPaths paths;

        private readonly IList<(DateTime timestamp, string name, bool success)> invokedCommands;

        protected CommandService(ILogger<CommandService> log, IPaths paths)
        {
            this.log = log;
            this.paths = paths;
            invokedCommands = new List<(DateTime timestamp, string name, bool success)>();
        }

        public void InvokeIfExists(string command)
        {
            if(CommandExists(command))
                InvokeCommand(command);
        }

        public abstract void InvokeCommand(string command);
        // {
        //     var isSubbed = proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult();
        //     
        //     if (isSubbed)
        //     {
        //         invokedCommands.Add((DateTime.Now, command, true));
        //         proxy.GetProxy().Commands.Invoke(command);      
        //     }
        //     else
        //     {
        //         invokedCommands.Add((DateTime.Now, command, false));
        //         log.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
        //     }
        //     
        //     var commandsPath = Path.Combine(paths.PluginDirectory.FullName, "Commands");
        //     Directory.CreateDirectory(commandsPath);
        //
        //     File.WriteAllLines(Path.Combine(commandsPath, "InvokedCommands.txt"), invokedCommands.Select(x=> $"{x.timestamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} {x.name}"));
        // }
        public abstract bool CommandExists(string command);
    }
}