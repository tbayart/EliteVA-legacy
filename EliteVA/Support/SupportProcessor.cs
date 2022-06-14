using EliteAPI.Abstractions;
using EliteAPI.Status.Processor.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Services;
using EliteVA.Support.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EliteVA.Support
{
    public class SupportProcessor : ISupportProcessor
    {
        private readonly ILogger<SupportProcessor> log;
        private readonly IEliteDangerousApi api;
        private readonly IStatusProcessor status;
        private readonly IFormatting formats;
        private readonly IVariableService variables;
        private readonly ICommandService commands;

        public SupportProcessor(ILogger<SupportProcessor> log, IEliteDangerousApi api, IStatusProcessor status, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            this.log = log;
            this.api = api;
            this.status = status;
            this.formats = formats;
            this.variables = variables;
            this.commands = commands;
        }
        
        public void Bind()
        {
            status.CargoUpdated += (sender, e) => SetVariablesAndInvoke("Status.Cargo", e.Cargo);
            status.MarketUpdated += (sender, e) => SetVariablesAndInvoke("Status.Market", e.Market);
            status.ModulesUpdated += (sender, e) => SetVariablesAndInvoke("Status.Modules", e.Modules);
            status.NavRouteUpdated += (sender, e) => SetVariablesAndInvoke("Status.NavRoute", e.NavRoute);
            status.OutfittingUpdated += (sender, e) => SetVariablesAndInvoke("Status.Outfitting", e.Outfitting);
            status.BackpackUpdated += (sender, e) => SetVariablesAndInvoke("Status.Backpack", e.Backpack);
            status.ShipyardUpdated += (sender, e) => SetVariablesAndInvoke("Status.Shipyard", e.Shipyard);
        }

        public IEnumerable<Variable> GetVariables(string name, object value)
        {
            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(value));
                var vars = variables.GetPaths(jObject);
                return vars.Select(x => new Variable(x.Name = formats.Support.ToVariable($"{name}.{x.Name}"), x.Value));
            }
            catch (JsonSerializationException ex)
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(value));
                var vars = variables.GetPaths(jArray);
                return vars.Select(x => new Variable(x.Name = formats.Support.ToVariable($"{name}.{x.Name}"), x.Value));
            }
            catch (Exception ex)
            {
                log.LogWarning("Could not get variables for {Name}", name);
                return new List<Variable>();
            }
        }

        public string GetCommand(string name)
        {
            return formats.Support.ToCommand(name);
        }

        void SetVariablesAndInvoke<T>(string name, T value)
        {
            var statusVariables = GetVariables(name, value);
            var command = GetCommand(name);

            variables.SetVariables(name, statusVariables);

            commands.InvokeCommand(command);
        }
    }
}