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
        #region fields
        private readonly ILogger _logger;
        private readonly IEliteDangerousApi _api;
        private readonly IFormatting _formats;
        private readonly IVariableService _variables;
        private readonly ICommandService _commands;
        #endregion fields

        #region ctor
        public EventProcessor(ILogger<EventProcessor> log, IEliteDangerousApi api, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            _logger = log;
            _api = api;
            _formats = formats;
            _variables = variables;
            _commands = commands;
        }
        #endregion ctor

        #region IEventProcessor
        /// <inheritdoc />
        public void Bind()
        {
            _api.Events.AllEvent += ProcessEvent;
        }

        public IEnumerable<Variable> GetVariables(string eventName, string json)
        {
            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(json);
                var vars = _variables.GetPaths(jObject, string.Empty);
                return vars.Select(x => new Variable(eventName, x.Name = _formats.Events.ToVariable(eventName, x.Name), x.Value));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not set variables for {Name} event", eventName);
                return new List<Variable>();
            }
        }

        /// <inheritdoc />
        public string GetCommand(IEvent e)
        {
            return _formats.Events.ToCommand(e);
        }
        #endregion IEventProcessor

        #region methods
        private void ProcessEvent(object sender, IEvent e)
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

                _variables.SetVariables("Events", variable);
                _commands.InvokeCommand(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not process {Event} event", e.Event);
            }
        }
        #endregion methods
    }
}