using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

using EliteAPI.Abstractions;
using EliteAPI.Event.Models;
using EliteAPI.Event.Models.Abstractions;

using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Event.Abstractions;
using EliteVA.Services;
using EliteVA.Services.Variable;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                    var variable = GetVariables(e);
                    var command = GetCommand(e);

                    variables.SetVariables(variable);

                    if (api.HasCatchedUp)
                    {
                        commands.InvokeCommand(command);
                    }
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Could not process {Event} event", e.Event);
                }
   
            };
        }

        /// <inheritdoc />
        public string GetCommand(IEvent e)
        {
            return formats.Events.ToCommand(e);
        }

        /// <inheritdoc />
        public IEnumerable<Variable> GetVariables(IEvent e)
        {
            try
            {
                if (e is NotImplementedEvent)
                {
                    log.LogDebug("Skipping {Name} event, it has not yet been implemented", e.Event);
                    return new List<Variable>();
                }

                return e.GetType().GetProperties().Where(x => x.Name != "Event" && x.Name != "Timestamp")
                    .SelectMany(x => GetVariables(e.Event, x.Name, x, e));
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Could not set variables for {Name} event", e.Event);
                return default;
            }
        }

        private IEnumerable<Variable> GetVariables(string eventName, string name, PropertyInfo property, object instance)
        {
            try
            {
                Type propertyType = property.PropertyType;
                TypeCode typeCode = Type.GetTypeCode(propertyType);

                switch (typeCode)
                {
                    case TypeCode.Object:
                        var value = property.GetValue(instance);
                        var properties = value.GetType().GetProperties();
                        return properties.SelectMany(p => GetVariables(eventName, $"{name}.{p.Name}", p, value));

                    default:
                        return new List<Variable>() {GetVariable(eventName, name, property.GetValue(instance))};
                }
            }
            catch { return Array.Empty<Variable>(); }
        }
        
        private Variable GetVariable<T>(string eventName, string name, T value)
        {
            string variableName = formats.Events.ToVariable(eventName, name);
            return new Variable(variableName, value);
        }
    }

}