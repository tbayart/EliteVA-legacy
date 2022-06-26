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
        #region fields
        private readonly ILogger _logger;
        private readonly IProxy _proxy;
        private readonly IPaths _paths;
        private readonly IEliteDangerousApi _api;
        private readonly IList<(DateTime timestamp, string name, bool success)> _invokedCommands;
        #endregion fields

        #region ctor
        public CommandService(ILogger<CommandService> log, IProxy proxy, IPaths paths, IEliteDangerousApi api)
        {
            _logger = log;
            _proxy = proxy;
            _paths = paths;
            _api = api;
            _invokedCommands = new List<(DateTime, string, bool)>();
        }
        #endregion ctor

        #region ICommandService
        public void InvokeCommand(string command)
        {
            if (_api.HasCatchedUp == false)
            {
                _logger.LogDebug("Skipping '{Command}' during catchup", command);
                return;
            }

            if (_proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult())
            {
                _invokedCommands.Add((DateTime.Now, command, true));
                _proxy.GetProxy().Commands.Invoke(command);
            }
            else
            {
                _invokedCommands.Add((DateTime.Now, command, false));
                _logger.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
            }

            var commandsPath = Path.Combine(_paths.PluginDirectory.FullName, "Commands");
            Directory.CreateDirectory(commandsPath);

            File.WriteAllLines(Path.Combine(commandsPath, "InvokedCommands.txt"), _invokedCommands.Select(x => $"{x.timestamp.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture)} {x.name}"));
        }
        #endregion ICommandService
    }
}
