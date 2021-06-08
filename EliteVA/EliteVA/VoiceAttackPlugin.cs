using Somfic.Logging.VoiceAttack;

using System;
using EliteVA.Commands;
using EliteVA.Variables;
using EliteVA.VoiceAttackProxy.Abstractions;
using Microsoft.Extensions.Hosting;
using Plugin;
using Formatting = Plugin.Constants.Formatting.Formatting;

namespace EliteVA
{
    public static class VoiceAttackPlugin
    {
        private static IHost _host;
        
        private static ElitePlugin _plugin;

        public static IVoiceAttackProxy Proxy;
        
        public static Guid VA_Id() => new Guid("189a4e44-caf1-459b-b62e-fabc60a12986");

        public static string VA_DisplayName() => "EliteAPI";

        public static string VA_DisplayInfo() => "EliteAPI by Somfic";

        public static void VA_Init1(dynamic vaProxy)
        {
            _plugin = new ElitePlugin();

            var builder = _plugin.CreateHostBuilder<Formatting, VariableService, CommandService>();
            builder.ConfigureLogging((context, logger) =>
            {
                VoiceAttackLoggerExtensions.AddVoiceAttack(logger, vaProxy);
            });

            _host = builder.Build();

            Proxy = new VoiceAttackProxy.VoiceAttackProxy(vaProxy, _host.Services);
            
            _plugin.Start(_host);
        }

        public static void VA_Exit1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy.VoiceAttackProxy(vaProxy, _host.Services);
        }

        public static void VA_StopCommand()
        {
            
        }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy.VoiceAttackProxy(vaProxy, _host.Services);
            _plugin.Execute(Proxy.Context);
        }
    }
}