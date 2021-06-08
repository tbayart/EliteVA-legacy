using System;
using System.Collections.Generic;
using System.Linq;
using EliteAPI.Abstractions;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Constants.Formatting.Abstractions;
using Plugin.Event.Abstractions;
using Plugin.Services.Command.Abstractions;
using Plugin.Services.Variable.Abstractions;

namespace Plugin.Event
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
                    
                    var eventVariables = GetVariables(e.Event, json);
                    var command = GetCommand(e);

                    foreach (var eventVariable in eventVariables)
                    {
                        variables.SetVariable("Events", eventVariable);
                    }

                    if (api.HasCatchedUp)
                    {
                        commands.InvokeIfExists(command);
                    }
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
                return vars.Select(x => new Variable(x.Name = formats.Events.ToVariable($"{eventName}.{x.Name}"), x.Value));
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
            return formats.Events.ToCommand(e.Event);
        }
    }
}