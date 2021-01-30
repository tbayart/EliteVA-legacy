using EliteAPI;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;

using EliteVA.ProfileGenerator.VoiceAttack;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Somfic.Logging.Console;
using Somfic.Logging.Console.Themes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

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
                    logger.AddPrettyConsole(ConsoleThemes.Code);
                })
                .ConfigureServices((context, service) =>
                {
                    service.AddEliteAPI();
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

        public Core(ILogger<Core> log)
        {
            _log = log;
        }

        public async Task Run()
        {
            Assembly eaAssembly = Assembly.GetAssembly(typeof(IEliteDangerousApi));

            if (eaAssembly == null)
            {
                _log.LogError("EliteAPI assembly was null");
                return;
            }

            List<Type> eventTypes = eaAssembly.GetTypes().Where(x => x.IsSubclassOf(typeof(EventBase)) && x.IsClass && !x.IsAbstract).ToList();

            if (!eventTypes.Any())
            {
                _log.LogWarning("No event classes could be found");
            }
            else
            {
                _log.LogInformation("Found {amount} event classes", eventTypes.Count());
            }

            Profile profile = new Profile();
            eventTypes.ForEach(x =>
            {
                string eventName = x.Name.Replace("Event", string.Empty);
                string name = $"((EliteAPI.{eventName}))";

                profile.AddCommand(new ProfileCommand(name, "EliteVA"));
            });

            _log.LogDebug("Writing to EliteVA.vap");

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Profile));
                TextWriter txtWriter = new StreamWriter("EliteVA.vap");
                xs.Serialize(txtWriter, profile);
                txtWriter.Close();

                _log.LogInformation("Profile created!");
            }
            catch (Exception ex)
            {
                _log.LogCritical(ex, "Could not create profile");
            }

            await Task.Delay(-1);
        }
    }
}
