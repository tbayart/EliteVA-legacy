using EliteAPI.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;
using System;
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
        private readonly string _commandsPath;
        #endregion fields

        #region ctor
        public CommandService(ILogger<CommandService> log, IProxy proxy, IPaths paths, IEliteDangerousApi api)
        {
            _logger = log;
            _proxy = proxy;
            _paths = paths;
            _api = api;
            _commandsPath = Path.Combine(_paths.PluginDirectory.FullName, "Commands");
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

            if (_proxy.GetProxy().Commands.Exists(command).GetAwaiter().GetResult())
            {
                _proxy.GetProxy().Commands.Invoke(command);
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
            var date = now.ToString("yyyy-MM-dd");
            var time = now.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);
            var commandsPath = Path.Combine(_commandsPath, $"InvokedCommands_{date}.txt");
            File.AppendAllLines(commandsPath, commands.Select(o => $"{time} {o}"));
        }
        #endregion methods
    }
}
