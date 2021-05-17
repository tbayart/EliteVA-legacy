using Somfic.Logging.VoiceAttack;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Options.Bindings;

using EliteVA.Bindings;
using EliteVA.Bindings.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Paths;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy;
using EliteVA.Constants.Proxy.Abstractions;
using EliteVA.Event;
using EliteVA.Event.Abstractions;
using EliteVA.Services;
using EliteVA.Services.Variable;
using EliteVA.Status;
using EliteVA.Status.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Valsom.Logging.File;
using Valsom.Logging.File.Formats;

using Formatting = EliteVA.Constants.Formatting.Formatting;


namespace EliteVA
{

    public class VoiceAttackPlugin
    {
        private static IHost Host { get; set; }
        private static IProxy Proxy { get; set; }
        private static string PluginDir { get; set; }
        
        public static Guid VA_Id() => new Guid("189a4e44-caf1-459b-b62e-fabc60a12986");

        public static string VA_DisplayName() => "EliteVA";

        public static string VA_DisplayInfo() => "EliteVA by Somfic";

        public static void VA_Init1(dynamic vaProxy)
        {
            Initialize(vaProxy);
        }

        public static void VA_Exit1(dynamic vaProxy)
        {
            Proxy.SetProxy(vaProxy);
        }

        public static void VA_StopCommand() { }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            Proxy.SetProxy(vaProxy);
        }

        private static void Initialize(dynamic vaProxy)
        {
            PluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory;

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IProxy, Proxy>();
                    
                    services.AddTransient<IFormatting, Formatting>();
                    services.AddTransient<IPaths, Paths>();

                    services.AddSingleton<IStatusProcessor, StatusProcessor>();
                    services.AddSingleton<IEventProcessor, EventProcessor>();
                    services.AddSingleton<IBindingsProcessor, BindingsProcessor>();

                    services.AddTransient<ICommandService, CommandService>();
                    services.AddSingleton<IVariableService, VariableService>();
                    
                    services.AddEliteAPI();
                })
                .ConfigureLogging((context, log) =>
                {
                    log.SetMinimumLevel(LogLevel.Trace);
                    VoiceAttackLoggerExtensions.AddVoiceAttack(log, vaProxy);
                    log.AddFile("EliteVA", PluginDir);
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddIniFile(Path.Combine(PluginDir, "EliteVA.ini"), true, true);
                })
                .Build();

            Proxy = Host.Services.GetRequiredService<IProxy>();
            
            Proxy.SetProxy(vaProxy);

            var api = Host.Services.GetRequiredService<IEliteDangerousApi>();

            var events = Host.Services.GetRequiredService<IEventProcessor>();
            var status = Host.Services.GetRequiredService<IStatusProcessor>();
            var bindings = Host.Services.GetRequiredService<IBindingsProcessor>();

            api.InitializeAsync().GetAwaiter().GetResult();
            
            events.Bind();
            status.Bind();
            bindings.Bind();

            api.StartAsync();
        }
    }
}