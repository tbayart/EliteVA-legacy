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
        private static IEliteDangerousAPI EliteAPI { get; set; }
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

            EliteAPI = Host.Services.GetService<IEliteDangerousAPI>();
            Log = Host.Services.GetService<ILogger<VoiceAttackPlugin>>();

            Log.LogDebug("EliteVA v{version}", Assembly.GetExecutingAssembly().GetName().Version);

            EliteAPI.Events.AllEvent += OnEliteDangerousEvent;

            EliteAPI.Status.Docked.OnChange += (sender, e) => SetStatus("Docked", e);
            EliteAPI.Status.Landed.OnChange += (sender, e) => SetStatus("Landed", e);
            EliteAPI.Status.Gear.OnChange += (sender, e) => SetStatus("Gear", e);
            EliteAPI.Status.Shields.OnChange += (sender, e) => SetStatus("Shields", e);
            EliteAPI.Status.Supercruise.OnChange += (sender, e) => SetStatus("Supercruise", e);
            EliteAPI.Status.FlightAssist.OnChange += (sender, e) => SetStatus("FlightAssist", e);
            EliteAPI.Status.Hardpoints.OnChange += (sender, e) => SetStatus("Hardpoints", e);
            EliteAPI.Status.Winging.OnChange += (sender, e) => SetStatus("Winging", e);
            EliteAPI.Status.Lights.OnChange += (sender, e) => SetStatus("Lights", e);
            EliteAPI.Status.CargoScoop.OnChange += (sender, e) => SetStatus("CargoScoop", e);
            EliteAPI.Status.SilentRunning.OnChange += (sender, e) => SetStatus("SilentRunning", e);
            EliteAPI.Status.Scooping.OnChange += (sender, e) => SetStatus("Scooping", e);
            EliteAPI.Status.SrvHandbreak.OnChange += (sender, e) => SetStatus("SrvHandbreak", e);
            EliteAPI.Status.MassLocked.OnChange += (sender, e) => SetStatus("MassLocked", e);
            EliteAPI.Status.FsdCharging.OnChange += (sender, e) => SetStatus("FsdCharging", e);
            EliteAPI.Status.FsdCooldown.OnChange += (sender, e) => SetStatus("FsdCooldown", e);
            EliteAPI.Status.LowFuel.OnChange += (sender, e) => SetStatus("LowFuel", e);
            EliteAPI.Status.Overheating.OnChange += (sender, e) => SetStatus("Overheating", e);
            EliteAPI.Status.HasLatLong.OnChange += (sender, e) => SetStatus("HasLatLong", e);
            EliteAPI.Status.InDanger.OnChange += (sender, e) => SetStatus("InDanger", e);
            EliteAPI.Status.InInterdiction.OnChange += (sender, e) => SetStatus("InInterdiction", e);
            EliteAPI.Status.InMothership.OnChange += (sender, e) => SetStatus("InMothership", e);
            EliteAPI.Status.InFighter.OnChange += (sender, e) => SetStatus("InFighter", e);
            EliteAPI.Status.InSrv.OnChange += (sender, e) => SetStatus("InSrv", e);
            EliteAPI.Status.AnalysisMode.OnChange += (sender, e) => SetStatus("AnalysisMode", e);
            EliteAPI.Status.NightVision.OnChange += (sender, e) => SetStatus("NightVision", e);
            EliteAPI.Status.AltitudeFromAverageRadius.OnChange += (sender, e) => SetStatus("AltitudeFromAverageRadius", e);
            EliteAPI.Status.FsdJump.OnChange += (sender, e) => SetStatus("FsdJump", e);
            EliteAPI.Status.SrvHighBeam.OnChange += (sender, e) => SetStatus("SrvHighBeam", e);
            EliteAPI.Status.Pips.OnChange += (sender, e) =>
            {
                SetStatus("Pips.Engines", e.Engines);
                SetStatus("Pips.System", e.System);
                SetStatus("Pips.Weapons", e.Weapons);
            };
            EliteAPI.Status.FireGroup.OnChange += (sender, e) => SetStatus("FireGroup", e);
            EliteAPI.Status.GuiFocus.OnChange += (sender, e) => SetStatus("GuiFocus", e.ToString());
            EliteAPI.Status.Fuel.OnChange += (sender, e) =>
            {
                SetStatus("Fuel.Main", e.Main);
                SetStatus("Fuel.Reservoir", e.Reservoir);
            };
            EliteAPI.Status.Cargo.OnChange += (sender, e) => SetStatus("Cargo", e);
            EliteAPI.Status.LegalState.OnChange += (sender, e) => SetStatus("LegalState", e.ToString());
            EliteAPI.Status.Latitude.OnChange += (sender, e) => SetStatus("Latitude", e);
            EliteAPI.Status.Altitude.OnChange += (sender, e) => SetStatus("Altitude", e);
            EliteAPI.Status.Longitude.OnChange += (sender, e) => SetStatus("Longitude", e);
            EliteAPI.Status.Heading.OnChange += (sender, e) => SetStatus("Heading", e);
            EliteAPI.Status.Body.OnChange += (sender, e) => SetStatus("Body", e);
            EliteAPI.Status.BodyRadius.OnChange += (sender, e) => SetStatus("BodyRadius", e);

            EliteAPI.StartAsync();
        }

        private static void SetStatus<T>(string name, T value)
        {
            name = $"EliteAPI.{name}";
            Proxy.Variables.Set(name, value);
        }

        private static void OnEliteDangerousEvent(object sender, EventBase e)
        {
            if (EliteAPI.HasCatchedUp)
            {
                if (HasBeenSubscribedTo(e))
                {
                    SetEventVariables(e);
                    TriggerEvent(e);
                }
                else
                {
                    string command = $"((EliteAPI.{e.Event}))";
                    Log.LogDebug("Skipping {event} event, '{command}' has not been subscribed to", e.Event, command);
                }
            }
        }

        static bool HasBeenSubscribedTo(EventBase e)
        {
            string command = $"((EliteAPI.{e.Event}))";

            return Proxy.Commands.Exists(command).GetAwaiter().GetResult();
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

        private static void SetEventVariables(IEvent e)
        {
            SetVariables(e, e.Event);
        }

        private static void SetVariables(object e, string name)
        {
            PropertyInfo[] properties = e.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                SetVariable(property, e, property.Name, name);
            }
        }

        private static void SetVariable(PropertyInfo property, object instance, string name, string eventName)
        {
            try
            {
                Type propertyType = property.PropertyType;
                TypeCode typeCode = Type.GetTypeCode(propertyType);

                switch (typeCode)
                {
                    case TypeCode.Empty:
                        Log.LogDebug("Skipping {property} in {name} event because the type was empty", name, eventName);
                        return;

                    case TypeCode.Object:
                        SetVariables(property.GetValue(instance), $"{name}.{property.Name}");
                        return;

                    default:
                        name = $"EliteAPI.Event.{name}";
                        var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                        var value = Convert.ChangeType(property.GetValue(instance), type);
                        Proxy.Variables.Set(name, value);
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning(ex, "Could not set {name}", name);
            }
        }

        private static void TriggerEvent(IEvent e)
        {
            TriggerCommand(e.Event);
        }

        private static async void TriggerCommand(string eventName)
        {
            eventName = eventName.Trim();
            string command = $"((EliteAPI.{eventName}))";

            if (await Proxy.Commands.Exists(command))
            {
                Log.LogDebug("Invoking '{command}' for {event} event", command, eventName);
                await Proxy.Commands.Invoke(command);
            }
        }
    }
}