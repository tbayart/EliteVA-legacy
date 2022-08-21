using EliteAPI.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;

namespace EliteVA.Services
{
    public class CommandService : ICommandService
    {
        #region fields
        private readonly ILogger _logger;
        private readonly IProxy _proxy;
        private readonly IEliteDangerousApi _api;
        private readonly string _commandsPath;
        #endregion fields

        #region ctor
        public CommandService(ILogger<CommandService> logger, IProxy proxy, IPaths paths, IEliteDangerousApi api)
        {
            _logger = logger;
            _proxy = proxy;
            _api = api;
            _commandsPath = Path.Combine(paths.PluginDirectory.FullName, "Commands");
            Directory.CreateDirectory(_commandsPath);
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

            var commands = _proxy.GetProxy().Commands;
            if (commands.Exists(command).GetAwaiter().GetResult())
            {
                commands.Invoke(command);
            }
            else
            {
                _logger.LogDebug("Skipping '{Command}' because it has not been subscribed to", command);
            }

            AppendInvokedCommands(command);
        }
        #endregion ICommandService

        #region methods
        private void AppendInvokedCommands(params string[] commands)
        {
            var now = DateTime.Now;
            var commandsPath = Path.Combine(_commandsPath, $"InvokedCommands_{now:yyyy-MM-dd}.txt");
            File.AppendAllLines(commandsPath, commands.Select(o => $"{now:HH:mm:ss.fff} {o}"));
        }
        #endregion methods
    }
}
