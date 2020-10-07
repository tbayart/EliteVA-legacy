using Newtonsoft.Json;
using Somfic.VoiceAttack.Proxy;
using Somfic.VoiceAttack.Proxy.Abstractions;
using System;
using System.IO;
using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;
using FileLogger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Somfic.VoiceAttack.Log;

namespace EliteVA
{
    public class VoiceAttackPlugin
    {
        private static IVoiceAttackProxy Proxy { get; set; }

        private static IHost Host { get; set; }
        private static IEliteDangerousAPI EliteAPI { get; set; }

        public static Guid VA_Id() => new Guid("189a4e44-caf1-459b-b62e-fabc60a12986");

        public static string VA_DisplayName() => "EliteVA";

        public static string VA_DisplayInfo() => "EliteVA by Somfic";

        public static void VA_Init1(dynamic vaProxy)
        { 
            Proxy = new VoiceAttackProxy(vaProxy);
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddEliteAPI();
                })
                .ConfigureLogging((context, log) =>
                {
                    log.SetMinimumLevel(LogLevel.Trace);
                    log.AddEventLog(settings =>
                    {
                        settings.SourceName = "EliteAPI";
                        settings.Filter = (s, level) => true;
                    });

                    log.AddFileLogger(config =>
                        config.FileName =
                            "C:\\Program Files (x86)\\Steam\\steamapps\\common\\VoiceAttack\\EliteAPI.txt");
                })
                .Build();

            EliteAPI = (IEliteDangerousAPI)Host.Services.GetService(typeof(IEliteDangerousAPI));

            EliteAPI.StartAsync();


            EliteAPI.Events.AllEvent += OnEliteDangerousEvent;
        }

        private static void OnEliteDangerousEvent(object sender, EventBase e)
        {
            Proxy.Log.Write(JsonConvert.SerializeObject(e), VoiceAttackColor.Black);
        }

        public static void VA_Exit1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy(vaProxy);
        }

        public static void VA_StopCommand()
        {

        }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy(vaProxy);
        }
    }
}