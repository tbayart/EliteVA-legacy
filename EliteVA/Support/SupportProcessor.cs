using EliteAPI.Abstractions;
using EliteAPI.Status.Processor.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Services;
using EliteVA.Support.Abstractions;
using EliteVA.VoiceAttackProxy.Variables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EliteVA.Support
{
    public class SupportProcessor : ISupportProcessor
    {
        #region fields
        private readonly ILogger<SupportProcessor> _logger;
        private readonly IStatusProcessor _status;
        private readonly IFormatting _formats;
        private readonly IVariableService _variables;
        private readonly ICommandService _commands;
        #endregion fields

        #region ctor
        public SupportProcessor(ILogger<SupportProcessor> log, IStatusProcessor status, IFormatting formats, IVariableService variables, ICommandService commands)
        {
            _logger = log;
            _status = status;
            _formats = formats;
            _variables = variables;
            _commands = commands;
        }
        #endregion ctor

        #region ISupportProcessor
        public void Bind()
        {
            _status.CargoUpdated += (sender, e) => SetVariablesAndInvoke("Status.Cargo", e.Cargo);
            _status.MarketUpdated += (sender, e) => SetVariablesAndInvoke("Status.Market", e.Market);
            _status.ModulesUpdated += (sender, e) => SetVariablesAndInvoke("Status.Modules", e.Modules);
            _status.NavRouteUpdated += (sender, e) => SetVariablesAndInvoke("Status.NavRoute", e.NavRoute);
            _status.OutfittingUpdated += (sender, e) => SetVariablesAndInvoke("Status.Outfitting", e.Outfitting);
            _status.BackpackUpdated += (sender, e) => SetVariablesAndInvoke("Status.Backpack", e.Backpack);
            _status.ShipyardUpdated += (sender, e) => SetVariablesAndInvoke("Status.Shipyard", e.Shipyard);
        }

        public IEnumerable<Variable> GetVariables(string name, object value)
        {
            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(value));
                var vars = _variables.GetPaths(jObject, name);
                return vars.Select(x => new Variable(name, _formats.Support.ToVariable($"{name}.{x.Name}"), x.Value));
            }
            catch (JsonSerializationException)
            {
                var jArray = JsonConvert.DeserializeObject<JArray>(JsonConvert.SerializeObject(value));
                var vars = _variables.GetPaths(jArray, name);
                return vars.Select(x => new Variable(name, _formats.Support.ToVariable($"{name}.{x.Name}"), x.Value));
            }
            catch
            {
                _logger.LogError("Could not get variables for {Name}", name);
                return new List<Variable>();
            }
        }

        public string GetCommand(string name)
        {
            return _formats.Support.ToCommand(name);
        }
        #endregion ISupportProcessor

        #region methods
        void SetVariablesAndInvoke<T>(string name, T value)
        {
            if (value != null)
            {
                var statusVariables = GetVariables(name, value);
                _variables.SetVariables(name, statusVariables);
            }
            var command = GetCommand(name);
            _commands.InvokeCommand(command);
        }
        #endregion methods
    }
}
