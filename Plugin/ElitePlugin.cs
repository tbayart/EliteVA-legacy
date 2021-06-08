using System;
using System.IO;
using System.Linq;
using System.Reflection;
using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Spansh;
using EliteAPI.Spansh.NeutronPlotter.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Bindings;
using Plugin.Bindings.Abstractions;
using Plugin.Constants.Formatting.Abstractions;
using Plugin.Constants.Paths;
using Plugin.Constants.Paths.Abstractions;
using Plugin.Event;
using Plugin.Services.Command.Abstractions;
using Plugin.Services.Variable.Abstractions;
using Plugin.Support;
using Plugin.Support.Abstractions;
using Valsom.Logging.File;
using IEventProcessor = Plugin.Event.Abstractions.IEventProcessor;
using IStatusProcessor = Plugin.Status.Abstractions.IStatusProcessor;
using StatusProcessor = Plugin.Status.StatusProcessor;

namespace Plugin
{
    public class ElitePlugin
    {
        private string _pluginDir;

        private INeutronPlotter _neutronPlotter;
        private IVariableService _variables;
        private IEventProcessor _eventProcessor;
        private ISupportProcessor _supportProcessor;
        private IStatusProcessor _statusProcessor;
        private IBindingsProcessor _bindingsProcessor;
        private IEliteDangerousApi _api;

        public void Start(IHost host)
        {
            _pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                         AppDomain.CurrentDomain.BaseDirectory;

            _api = host.Services.GetRequiredService<IEliteDangerousApi>();
            _eventProcessor = host.Services.GetRequiredService<IEventProcessor>();
            _neutronPlotter = host.Services.GetRequiredService<INeutronPlotter>();
            _variables = host.Services.GetRequiredService<IVariableService>();
            _statusProcessor = host.Services.GetRequiredService<IStatusProcessor>();
            _supportProcessor = host.Services.GetRequiredService<ISupportProcessor>();
            _bindingsProcessor = host.Services.GetRequiredService<IBindingsProcessor>();
            
            _api.InitializeAsync();
            
            _eventProcessor.Bind();
            _statusProcessor.Bind();
            _supportProcessor.Bind();
            _bindingsProcessor.Bind();

            _api.StartAsync();
        }

        public void Execute(string name)
        {
            if (name == "Spansh.NeutronPlotter")
            {
                string sourceSystem = _variables.GetVariable<string>("EliteAPI.Spansh.NeutronPlotter.SourceSystem");
                string targetSystem = _variables.GetVariable<string>("EliteAPI.Spansh.NeutronPlotter.TargetSystem");
                int efficiency = _variables.GetVariable<int>("EliteAPI.Spansh.NeutronPlotter.Efficiency");
                int range = _variables.GetVariable<int>("EliteAPI.Spansh.NeutronPlotter.Range");

                var result = _neutronPlotter.Plot(sourceSystem, targetSystem, range, efficiency).GetAwaiter()
                    .GetResult().Result;

                var variables = _variables
                    .GetPaths(JObject.FromObject(result,
                        new JsonSerializer {ContractResolver = new LongNameContractResolver()})).Select(x =>
                        new Variable($"EliteAPI.Spansh.NeutronPlotter.{x.Name}", x.Value));

                foreach (var variable in variables)
                {
                    _variables.SetVariable("ThirdParty.Spansh", variable);
                }
            }
        }

        public IHostBuilder CreateHostBuilder<TFormatting, TVariableService, TCommandService>() where TFormatting : class, IFormatting where TVariableService : class, IVariableService where TCommandService : class, ICommandService
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IPaths, Paths>();

                    services.AddSingleton<IStatusProcessor, StatusProcessor>();
                    services.AddSingleton<ISupportProcessor, SupportProcessor>();
                    services.AddSingleton<IEventProcessor, EventProcessor>();
                    services.AddSingleton<IBindingsProcessor, BindingsProcessor>();

                    services.AddSingleton<ICommandService, TCommandService>();
                    services.AddSingleton<IVariableService, TVariableService>();
                    services.AddSingleton<IFormatting, TFormatting>();

                    services.AddEliteAPI();
                    services.AddSpansh();

                    // Remove annoying HttpClient messages
                    services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
                })
                .ConfigureLogging((context, log) =>
                {
                    log.SetMinimumLevel(LogLevel.Trace);
                    log.AddFile("EliteAPI", Path.Combine(_pluginDir, "Logs"));
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddIniFile(Path.Combine(_pluginDir, "EliteAPI.ini"), true, true);
                });
        }
    }
}