using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Spansh;
using EliteAPI.Spansh.NeutronPlotter.Abstractions;
using EliteVA.Bindings;
using EliteVA.Bindings.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Paths;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy;
using EliteVA.Constants.Proxy.Abstractions;
using EliteVA.Event;
using EliteVA.Event.Abstractions;
using EliteVA.Logging;
using EliteVA.Services;
using EliteVA.Status;
using EliteVA.Status.Abstractions;
using EliteVA.Support;
using EliteVA.Support.Abstractions;
using EliteVA.VoiceAttackProxy.Variables;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Formatting = EliteVA.Constants.Formatting.Formatting;

namespace EliteVA
{
    public class VoiceAttackPlugin
    {
#if DEBUG
        static VoiceAttackPlugin()
        {
            System.Diagnostics.Debugger.Launch();
        }
#endif //DEBUG

        private static Stopwatch _catchupTime = Stopwatch.StartNew();

        private static IHost Host { get; set; }

        private static IProxy Proxy { get; set; }

        private static IVariableService Variables { get; set; }

        public static ILogger<VoiceAttackPlugin> Log { get; set; }

        private static string PluginDir { get; set; }

        public static Guid VA_Id() => new Guid("189a4e44-caf1-459b-b62e-fabc60a12986");

        public static string VA_DisplayName() => "EliteVA";

        public static string VA_DisplayInfo() => "EliteVA by Somfic";

        public static void VA_Init1(dynamic vaProxy)
        {
            try
            {
                Initialize(vaProxy);
            }
            catch (Exception ex)
            {
                File.WriteAllText("eliteva.error.json", JsonConvert.SerializeObject(ex));
            }
        }

        public static void VA_Exit1(dynamic vaProxy)
        {
            Proxy.SetProxy(vaProxy);
            try
            {
                var alf = Host.Services.GetRequiredService<IHostApplicationLifetime>();
                alf.StopApplication();
            }
            catch (Exception ex)
            {
                _ = ex;
            }
            Host.StopAsync().GetAwaiter().GetResult();
        }

        public static void VA_StopCommand() { }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            Proxy.SetProxy(vaProxy);

            var proxy = Proxy.GetProxy();

            if (proxy.Context == "Spansh.NeutronPlotter")
            {
                string sourceSystem = proxy.Variables.Get<string>("EliteAPI.Spansh.NeutronPlotter.SourceSystem");
                string targetSystem = proxy.Variables.Get<string>("EliteAPI.Spansh.NeutronPlotter.TargetSystem");
                int efficiency = proxy.Variables.Get<int>("EliteAPI.Spansh.NeutronPlotter.Efficiency");
                int range = proxy.Variables.Get<int>("EliteAPI.Spansh.NeutronPlotter.Range");

                var neutronPlotter = Host.Services.GetRequiredService<INeutronPlotter>();
                var result = neutronPlotter.Plot(sourceSystem, targetSystem, range, efficiency).GetAwaiter().GetResult().Result;
                Variables.SetVariables("ThirdParty.Spansh", Variables.GetPaths(JObject.FromObject(result, new JsonSerializer { ContractResolver = new LongNameContractResolver() }), string.Empty).Select(x => new Variable(string.Empty, $"EliteAPI.Spansh.NeutronPlotter.{x.Name}", x.Value)));
            }

            try
            {
                if (proxy.Context == "EliteVA.LandingPad.Show")
                {
                    var root = "EliteAPI.DockingGranted.";
                    var stationName = proxy.Variables.Get<string>(root + "StationName");
                    if (string.IsNullOrEmpty(stationName))
                        return;

                    var stationType = proxy.Variables.Get<string>(root + "StationType");
                    var landingPad = proxy.Variables.Get<int>(root + "LandingPad");

                    Host.Services
                        .GetRequiredService<GUI.WindowManager>()
                        .LandingPadShow(stationName, stationType, landingPad);
                }
                else if (proxy.Context == "EliteVA.LandingPad.Hide")
                {
                    var wm = Host.Services
                        .GetRequiredService<GUI.WindowManager>();
                    wm.LandingPadHide();
                }
            }
            catch (Exception ex)
            {
                var logger = Host.Services.GetRequiredService<ILogger<VoiceAttackPlugin>>();
                logger.Log(LogLevel.Error, ex, "Exception on {Context}", proxy.Context);
            }
        }

        private static void Initialize(dynamic vaProxy)
        {
            PluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<IProxy, Proxy>();

                    services.AddTransient<IFormatting, Formatting>();
                    services.AddTransient<IPaths, Paths>();

                    services.AddSingleton<IStatusProcessor, StatusProcessor>();
                    services.AddSingleton<ISupportProcessor, SupportProcessor>();
                    services.AddSingleton<IEventProcessor, EventProcessor>();
                    services.AddSingleton<IBindingsProcessor, BindingsProcessor>();

                    services.AddSingleton<ICommandService, CommandService>();
                    services.AddSingleton<IVariableService, VariableService>();

                    services.AddEliteAPI();
                    services.AddSpansh();

                    services.AddSingleton<GUI.WindowManager>();

                    // Remove annoying HttpClient messages
                    //services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
                })
                .ConfigureLogging((context, loggingBuilder) =>
                {
                    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
                    VALoggingExtensions.AddVoiceAttack(loggingBuilder, vaProxy);
                    loggingBuilder.AddLogger();
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddIniFile(Path.Combine(PluginDir, "EliteVA.ini"), true, true);
                })
                .Build();

            _ = Host.RunAsync();
            Proxy = Host.Services.GetRequiredService<IProxy>();
            Proxy.SetProxy(vaProxy);

            Host.Services.GetRequiredService<GUI.WindowManager>();

            var api = Host.Services.GetRequiredService<IEliteDangerousApi>();
            api.OnCatchedUp += EliteApiCatchedUp;

            var events = Host.Services.GetRequiredService<IEventProcessor>();
            var status = Host.Services.GetRequiredService<IStatusProcessor>();
            var support = Host.Services.GetRequiredService<ISupportProcessor>();
            var bindings = Host.Services.GetRequiredService<IBindingsProcessor>();
            Variables = Host.Services.GetRequiredService<IVariableService>();
            Log = Host.Services.GetRequiredService<ILogger<VoiceAttackPlugin>>();

            api.InitializeAsync().GetAwaiter().GetResult();

            events.Bind();
            status.Bind();
            support.Bind();
            bindings.Bind();

            api.StartAsync();
        }

        private static void EliteApiCatchedUp(object sender, EventArgs e)
        {
            Proxy.GetProxy().Log.Write($"Catchup done in {_catchupTime.Elapsed}", VoiceAttackProxy.Log.VoiceAttackColor.Pink);
            Host.Services.GetRequiredService<IEliteDangerousApi>().OnCatchedUp -= EliteApiCatchedUp;
        }
    }
}
