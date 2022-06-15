using EliteAPI.Abstractions;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Event.Abstractions;
using EliteVA.Services;
using EliteVA.VoiceAttackProxy.Variables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteVA.Event
{
    public class EventProcessor : IEventProcessor
    {
        private readonly ILogger<EventProcessor> log;
        private readonly IEliteDangerousApi api;
        private readonly IFormatting formats;
        private readonly IVariableService variables;
        private readonly ICommandService commands;

        public EventProcessor(ILogger<EventProcessor> log, IEliteDangerousApi api, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            this.log = log;
            this.api = api;
            this.formats = formats;
            this.variables = variables;
            this.commands = commands;
        }

        /// <inheritdoc />
        public void Bind()
        {
            api.Events.AllEvent += (sender, e) =>
            {
                try
                {
                    string json = e.ToJson();

                    if (e is NotImplementedEvent notImplemented)
                    {
                        json = notImplemented.Json;
                    }
                    
                    var variable = GetVariables(e.Event, json);
                    var command = GetCommand(e);

                    variables.SetVariables("Events", variable);
                    commands.InvokeCommand(command);
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Could not process {Event} event", e.Event);
                }
            };
        }

        public IEnumerable<Variable> GetVariables(string eventName, string json)
        {
            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(json);
                var vars = variables.GetPaths(jObject);
                return vars.Select(x => new Variable(x.Name = formats.Events.ToVariable(eventName, x.Name), x.Value));
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Could not set variables for {Name} event", eventName);
                return new List<Variable>();
            }
        }

        /// <inheritdoc />
        public string GetCommand(IEvent e)
        {
            return formats.Events.ToCommand(e);
        }
    }
}