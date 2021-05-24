using Somfic.Logging.VoiceAttack;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Autotrade.EDDB;
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
using EliteVA.EDDB;
using EliteVA.Event;
using EliteVA.Event.Abstractions;
using EliteVA.Services;
using EliteVA.Services.Variable;
using EliteVA.Status;
using EliteVA.Status.Abstractions;
using EliteVA.Support;
using EliteVA.Support.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Valsom.Logging.File;
using Valsom.Logging.File.Formats;

using Formatting = EliteVA.Constants.Formatting.Formatting;


namespace EliteVA
{

    public class VoiceAttackPlugin
    {
        private static IHost Host { get; set; }
        private static IProxy Proxy { get; set; }
        
        private static IVariableService Variables { get; set; }
        
        public static ILogger<VoiceAttackPlugin> Log { get; set; }
        
        private static HttpClient Client { get; set; }
        
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
        }

        public static void VA_StopCommand() { }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            // try
            // {
            //     var proxy = Proxy.SetProxy(vaProxy);
            //
            //     if (proxy.Context.ToLower() == "eddb.route")
            //     {
            //         var starSystem = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.StarSystem");
            //         var startStation = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.StartStation");
            //         var maxHopDistance = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.MaxHopDistance");
            //         var hopCount = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.HopCount");
            //         var cargoCapacity = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.CargoCapacity");
            //         var availableCredits = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.AvailableCredits");
            //         var pad = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.Pad");
            //         var maxDistance = proxy.Variables.Get<int>("EliteAPI.EDDB.Request.MaxDistance");
            //         var planetary = proxy.Variables.Get<bool>("EliteAPI.EDDB.Request.Planetary");
            //
            //         var hopsRequest = new HopsRequest(starSystem, startStation, maxHopDistance, hopCount, cargoCapacity, availableCredits, pad, maxDistance, planetary);
            //         var content = JsonConvert.SerializeObject(hopsRequest);
            //         var result = Client.PostAsync("https://eddb.io/route/search/hops",
            //             new StringContent(content, Encoding.UTF8, "application/json")).GetAwaiter().GetResult();
            //         result.EnsureSuccessStatusCode();
            //         var response =
            //             JsonConvert.DeserializeObject<JObject>(result.Content.ReadAsStringAsync().GetAwaiter()
            //                 .GetResult());
            //         
            //         Variables.SetVariables(Variables.GetPaths(response).Select(x => new Variable($"EliteAPI.EDDB.Response.{x.Name}", x.Value)));
            //     }
            //
            //     else if (proxy.Context.ToLower() == "eddb.system")
            //     {
            //         string name = proxy.Variables.Get<string>("EliteAPI.EDDB.Request.Name");
            //
            //         var request = new HttpRequestMessage(HttpMethod.Get, "https://eddb.io/system/search?system%5Bname%5D=" + name);
            //         var result = Client.SendAsync(request).GetAwaiter().GetResult();
            //         
            //         result.EnsureSuccessStatusCode();
            //         var response =
            //             JsonConvert.DeserializeObject<JArray>(result.Content.ReadAsStringAsync().GetAwaiter()
            //                 .GetResult());
            //         
            //         Variables.SetVariables(Variables.GetPaths(response).Select(x => new Variable($"EliteAPI.EDDB.Response.{x.Name}", x.Value)));
            //     }
            // }
            // catch (Exception ex)
            // {
            //     Log.LogWarning(ex, "Could not execute function");
            // }
        }

        private static void Initialize(dynamic vaProxy)
        {
            PluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(Path.Combine(PluginDir, "Logs"));

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
                })
                .ConfigureLogging((context, log) =>
                {
                    log.SetMinimumLevel(LogLevel.Trace);
                    VoiceAttackLoggerExtensions.AddVoiceAttack(log, vaProxy);
                    log.AddFile("EliteVA", Path.Combine(PluginDir, "Logs"));
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddIniFile(Path.Combine(PluginDir, "EliteVA.ini"), true, true);
                })
                .Build();

            Proxy = Host.Services.GetRequiredService<IProxy>();
            Proxy.SetProxy(vaProxy);

            Client = new HttpClient();

            var api = Host.Services.GetRequiredService<IEliteDangerousApi>();

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
    }
}