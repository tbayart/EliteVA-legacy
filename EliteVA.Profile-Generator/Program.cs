using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;
using EliteAPI.Status.Abstractions;
using EliteAPI.Status.Commander.Abstractions;
using EliteAPI.Status.Ship.Abstractions;
using EliteVA.Constants.Formatting;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.ProfileGenerator.VoiceAttack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Valsom.Logging.PrettyConsole;
using Valsom.Logging.PrettyConsole.Formats;
using Valsom.Logging.PrettyConsole.Themes;

namespace EliteVA.ProfileGenerator
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder()
                .ConfigureLogging((context, logger) =>
                {
                    logger.ClearProviders();
                    logger.SetMinimumLevel(LogLevel.Trace);
                    logger.AddPrettyConsole(ConsoleFormats.Default, ConsoleThemes.Vanilla);
                })
                .ConfigureServices((context, service) =>
                {
                    service.AddEliteAPI();
                    service.AddTransient<IFormatting, Formatting>();
                })
                .Build();


            Core core = ActivatorUtilities.CreateInstance<Core>(host.Services);

            await core.Run();

            await Task.Delay(-1);
        }
    }

    public class Core
    {
        private readonly ILogger<Core> _log;
        private readonly IFormatting format;

        public Core(ILogger<Core> log, IFormatting format)
        {
            _log = log;
            this.format = format;
        }

        public async Task Run()
        {
            Assembly eaAssembly = Assembly.GetAssembly(typeof(IEliteDangerousApi));

            if (eaAssembly == null)
            {
                _log.LogCritical("EliteAPI assembly was null");
                return;
            }

            List<Type> eventTypes = eaAssembly.GetTypes().Where(x => typeof(IEvent).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && !x.IsInterface && !x.Name.StartsWith("Raw")).ToList();
            List<PropertyInfo> shipVars = typeof(IShip).GetProperties().ToList();
            shipVars.AddRange(typeof(ICommander).GetProperties().ToList());
            shipVars = shipVars.Where(x => !x.Name.Contains("Flags")).ToList();

            List<Type> shipEvents = eaAssembly.GetTypes().Where(x => typeof(IStatus).IsAssignableFrom(x) && x.IsClass && !x.IsAbstract && !x.IsInterface).ToList();

            if (!shipVars.Any()) { _log.LogWarning("No ship events could be found"); }
            else { _log.LogInformation("Found {amount} ship events", shipVars.Count); }
            
            if (!shipEvents.Any()) { _log.LogWarning("No support events could be found"); }
            else { _log.LogInformation("Found {amount} support events", shipEvents.Count); }
            
            if (!eventTypes.Any()) { _log.LogWarning("No game events could be found"); }
            else { _log.LogInformation("Found {amount} game events", eventTypes.Count); }
            
            Profile profile = new Profile();
            shipVars.ForEach(x =>
            {
                string name = format.Status.ToCommand(x.Name);
                
                profile.AddCommand(new ProfileCommand(name, "EliteVA - Ship events"));
            });
            
            shipEvents.ForEach(x =>
            {
                string name = format.Status.ToCommand(x.Name);

                profile.AddCommand(new ProfileCommand(name, $"EliteVA - Support events"));
            });

            eventTypes.ForEach(x =>
            {
                string eventName = x.Name.Replace("Event", string.Empty);
                string name = format.Events.ToCommand(eventName);

                profile.AddCommand(new ProfileCommand(name, $"EliteVA - Game events"));
            });

            _log.LogDebug("Writing to EliteVA.vap");

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                TextWriter txtWriter = new StreamWriter(@"C:\Program Files (x86)\Steam\steamapps\common\VoiceAttack\Apps\EliteVA\EliteVA.vap");
                xs.Serialize(txtWriter, profile);
                txtWriter.Close();

                _log.LogInformation("Profile created!");
            }
            catch (Exception ex) { _log.LogCritical(ex, "Could not create profile"); }

            await Task.Delay(-1);
        }
    }
}