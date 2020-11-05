using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;

using FileLogger;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Somfic.VoiceAttack.Proxy;
using Somfic.VoiceAttack.Proxy.Abstractions;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Somfic.Logging.VoiceAttack;

namespace EliteVA
{
    public class VoiceAttackPlugin
    {
        private static IVoiceAttackProxy Proxy { get; set; }
        private static IHost Host { get; set; }
        private static IEliteDangerousAPI Api { get; set; }
        private static ILogger<VoiceAttackPlugin> Log { get; set; }

        public static Guid VA_Id()
        {
            return new Guid("189a4e44-caf1-459b-b62e-fabc60a12986");
        }

        public static string VA_DisplayName()
        {
            return "EliteVA";
        }

        public static string VA_DisplayInfo()
        {
            return "EliteVA by Somfic";
        }

        public static void VA_Init1(dynamic vaProxy)
        {
            string pluginDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory;

            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddEliteAPI();
                })
                .ConfigureLogging((context, log) =>
                {
                    log.SetMinimumLevel(LogLevel.Trace);
                    VoiceAttackLoggerExtensions.AddVoiceAttack(log, vaProxy);

                    log.AddFileLogger(config =>
                    {
                        config.FileName = Path.Combine(pluginDir, "EliteVA.log");
                        config.LogLevel = LogLevel.Trace;
                    });

                })
                .Build();

            Proxy = new VoiceAttackProxy(vaProxy, Host.Services);

            Api = Host.Services.GetService<IEliteDangerousAPI>();
            Log = Host.Services.GetService<ILogger<VoiceAttackPlugin>>();

            Log.LogDebug("EliteVA v{version}", Assembly.GetExecutingAssembly().GetName().Version);

            SubscribeToEvents();

            Api.StartAsync();
        }

        public static void VA_Exit1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy(vaProxy, Host.Services);
        }

        public static void VA_StopCommand()
        {

        }

        public static void VA_Invoke1(dynamic vaProxy)
        {
            Proxy = new VoiceAttackProxy(vaProxy, Host.Services);
        }

        private static void SubscribeToEvents()
        {
            Api.Events.AllEvent += OnEliteDangerousEvent;
            Api.Status.Docked.OnChange += (sender, e) => SetStatus("Docked", e);
            Api.Status.Landed.OnChange += (sender, e) => SetStatus("Landed", e);
            Api.Status.Gear.OnChange += (sender, e) => SetStatus("Gear", e);
            Api.Status.Shields.OnChange += (sender, e) => SetStatus("Shields", e);
            Api.Status.Supercruise.OnChange += (sender, e) => SetStatus("Supercruise", e);
            Api.Status.FlightAssist.OnChange += (sender, e) => SetStatus("FlightAssist", e);
            Api.Status.Hardpoints.OnChange += (sender, e) => SetStatus("Hardpoints", e);
            Api.Status.Winging.OnChange += (sender, e) => SetStatus("Winging", e);
            Api.Status.Lights.OnChange += (sender, e) => SetStatus("Lights", e);
            Api.Status.CargoScoop.OnChange += (sender, e) => SetStatus("CargoScoop", e);
            Api.Status.SilentRunning.OnChange += (sender, e) => SetStatus("SilentRunning", e);
            Api.Status.Scooping.OnChange += (sender, e) => SetStatus("Scooping", e);
            Api.Status.SrvHandbreak.OnChange += (sender, e) => SetStatus("SrvHandbreak", e);
            Api.Status.MassLocked.OnChange += (sender, e) => SetStatus("MassLocked", e);
            Api.Status.FsdCharging.OnChange += (sender, e) => SetStatus("FsdCharging", e);
            Api.Status.FsdCooldown.OnChange += (sender, e) => SetStatus("FsdCooldown", e);
            Api.Status.LowFuel.OnChange += (sender, e) => SetStatus("LowFuel", e);
            Api.Status.Overheating.OnChange += (sender, e) => SetStatus("Overheating", e);
            Api.Status.HasLatLong.OnChange += (sender, e) => SetStatus("HasLatLong", e);
            Api.Status.InDanger.OnChange += (sender, e) => SetStatus("InDanger", e);
            Api.Status.InInterdiction.OnChange += (sender, e) => SetStatus("InInterdiction", e);
            Api.Status.InMothership.OnChange += (sender, e) => SetStatus("InMothership", e);
            Api.Status.InFighter.OnChange += (sender, e) => SetStatus("InFighter", e);
            Api.Status.InSrv.OnChange += (sender, e) => SetStatus("InSrv", e);
            Api.Status.AnalysisMode.OnChange += (sender, e) => SetStatus("AnalysisMode", e);
            Api.Status.NightVision.OnChange += (sender, e) => SetStatus("NightVision", e);
            Api.Status.AltitudeFromAverageRadius.OnChange += (sender, e) => SetStatus("AltitudeFromAverageRadius", e);
            Api.Status.FsdJump.OnChange += (sender, e) => SetStatus("FsdJump", e);
            Api.Status.SrvHighBeam.OnChange += (sender, e) => SetStatus("SrvHighBeam", e);
            Api.Status.Pips.OnChange += (sender, e) =>
            {
                SetStatus("Pips.Engines", e.Engines, false);
                SetStatus("Pips.System", e.System, false);
                SetStatus("Pips.Weapons", e.Weapons, false);
                SetStatus("Pips", string.Empty);
            };
            Api.Status.FireGroup.OnChange += (sender, e) => SetStatus("FireGroup", e);
            Api.Status.GuiFocus.OnChange += (sender, e) => SetStatus("GuiFocus", e.ToString());
            Api.Status.Fuel.OnChange += (sender, e) =>
            {
                SetStatus("Fuel.Main", e.Main, false);
                SetStatus("Fuel.Reservoir", e.Reservoir, false);
                SetStatus("Fuel", string.Empty);
            };
            Api.Status.Cargo.OnChange += (sender, e) => SetStatus("Cargo", e);
            Api.Status.LegalState.OnChange += (sender, e) => SetStatus("LegalState", e.ToString());
            Api.Status.Latitude.OnChange += (sender, e) => SetStatus("Latitude", e);
            Api.Status.Altitude.OnChange += (sender, e) => SetStatus("Altitude", e);
            Api.Status.Longitude.OnChange += (sender, e) => SetStatus("Longitude", e);
            Api.Status.Heading.OnChange += (sender, e) => SetStatus("Heading", e);
            Api.Status.Body.OnChange += (sender, e) => SetStatus("Body", e);
            Api.Status.BodyRadius.OnChange += (sender, e) => SetStatus("BodyRadius", e);
        }

        static string ToEventCommand(IEvent e) => ToEventCommand(e.Event);
        static string ToEventCommand(string command) => $"((EliteAPI.{command}))";
        static string ToEventVariable(string variable) => $"EliteAPI.{variable}";

        static string ToStatusCommand(string command) => $"((EliteAPI.Status.{command}))";
        static string ToStatusVariable(string variable) => $"EliteAPI.{variable}";

        private static void SetStatus<T>(string name, T value, bool triggerChangeCommand = true)
        {
            var variable = ToStatusVariable(name);
            var command = ToStatusCommand(name);

            if (value.ToString() != string.Empty)
            {
                Proxy.Variables.Set(variable, value);
            }

            if (Api.HasCatchedUp && triggerChangeCommand)
            {
                TriggerCommand(command, $"{name} status");
            }
        }

        private static void OnEliteDangerousEvent(object sender, EventBase e)
        {
            string command = ToEventCommand(e);

            if (Api.HasCatchedUp)
            {
                if (HasBeenSubscribedTo(e))
                {
                    SetVariables(e);
                }

                TriggerCommand(command, $"{e.Event} event");
            }
        }

        static bool HasBeenSubscribedTo(IEvent e) => HasBeenSubscribedTo(ToEventCommand(e));

        static bool HasBeenSubscribedTo(string command) => Proxy.Commands.Exists(command).GetAwaiter().GetResult();

        private static void SetVariables(IEvent e) => SetVariables(e, e.Event);

        private static void SetVariables(object value, string eventName)
        { 
            var properties = value.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                SetVariable(property, value, property.Name, eventName);
            }
        }

        private static void SetVariable(PropertyInfo property, object instance, string name, string eventName)
        {
            try
            {
                var propertyType = property.PropertyType;
                var typeCode = Type.GetTypeCode(propertyType);

                switch (typeCode)
                {
                    case TypeCode.Empty:
                        Log.LogDebug("Could not set {property} in {name} event because the type was empty", name, eventName);
                        return;

                    case TypeCode.Object:
                        SetVariables(property.GetValue(instance), $"{name}.{property.Name}");
                        return;

                    default:
                        var variable = ToEventVariable(name);
                        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                        var value = Convert.ChangeType(property.GetValue(instance), type);
                        Proxy.Variables.Set(variable, value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.LogDebug(ex, "Could not set 'EliteAPI.Event.{name}'", name);
            }
        }

        private static void TriggerCommand(string command, string source)
        {
            if (Proxy.Commands.Exists(command).GetAwaiter().GetResult())
            {
                Log.LogDebug("Invoking '{command}' for {event}", command, source);
                Proxy.Commands.Invoke(command).GetAwaiter().GetResult();
            }
            else
            {
                Log.LogDebug("Skipping '{command}' for {event}", command, source);
            }
        }
    }
}