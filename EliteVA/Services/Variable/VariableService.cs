using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using EliteVA.VoiceAttackProxy.Variables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EliteVA.Services
{
    public class VariableService : IVariableService
    {
        #region fields
        private readonly ILogger _logger;
        private readonly IProxy _proxy;
        private readonly IPaths _paths;
        #endregion fields

        #region ctor
        public VariableService(ILogger<VariableService> logger, IProxy proxy, IPaths paths)
        {
            _logger = logger;
            _proxy = proxy;
            _paths = paths;
        }
        #endregion ctor

        /// <inheritdoc />
        public void SetVariable(string category, Variable variable)
        {
            try
            {
                _proxy.GetProxy().Variables.Set(category, variable);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not set variable {name} to {value}", variable.Name, variable.Value);
            }
        }

        /// <inheritdoc />
        public void SetVariables(string category, IEnumerable<Variable> variablesEnumerable)
        {
            try
            {
                var variables = variablesEnumerable.ToList();
                foreach (var variable in variables)
                {
                    SetVariable(category, variable);
                }

                var variablesPath = Path.Combine(_paths.PluginDirectory.FullName, "Variables");
                Directory.CreateDirectory(variablesPath);

                var setVariables = _proxy.GetProxy().Variables.SetVariables.GroupBy(x => x.Key.category).ToList();
                setVariables.ForEach(x =>
                    File.WriteAllLines(
                            Path.Combine(variablesPath, x.Key + ".txt"), x.Select(y => y.Key.name + ": " + y.Value.Value)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot save variables");
            }
        }

        public List<Variable> GetPaths(JObject jObject, string sourceEvent)
        {
            try
            {
                return jObject.Properties().SelectMany(o => GetPaths(o, sourceEvent)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot get event variable paths (object)");
                return new List<Variable>();
            }
        }

        public List<Variable> GetPaths(JArray jArray, string sourceEvent)
        {
            try
            {
                return jArray.Values<JObject>().SelectMany(x => x.Properties().SelectMany(o => GetPaths(o, sourceEvent))).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot get event variable paths (array)");
                return new List<Variable>();
            }
        }

        private List<Variable> GetPaths(JProperty property, string sourceEvent)
        {
            try
            {
                switch (property.Value.Type)
                {
                    case JTokenType.Object:
                        return property.Value.Children<JProperty>().SelectMany(o => GetPaths(o, sourceEvent)).ToList();

                    case JTokenType.Array:
                        return property.Value.Values<JObject>().SelectMany(o => GetPaths(o, sourceEvent)).ToList();

                    case JTokenType.Boolean:
                        return new List<Variable>
                            {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<bool>())};

                    case JTokenType.String:
                        return new List<Variable>
                            {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<string>())};

                    case JTokenType.Date:
                        return new List<Variable>
                            {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<DateTime>())};

                    case JTokenType.Integer:
                        try
                        {
                            return new List<Variable>
                                {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<int>())};
                        }
                        catch (OverflowException)
                        {
                            return new List<Variable>
                                {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<long>())};
                        }

                    case JTokenType.Float:
                        return new List<Variable>
                            {new Variable(sourceEvent,property.Value.Path, property.Value.ToObject<decimal>())};

                    default:
                        return new List<Variable>();
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogDebug(ex, "Could not process {Path}", property.Value.Path);
                return new List<Variable>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not process {Path}", property.Value.Path);
                return new List<Variable>();
            }
        }

        //public void ClearVariables(string category, string eventName)
        //{
        //    var variables = proxy.GetProxy().Variables.SetVariables
        //        .Where(o => o.Key.category == category)
        //        .Where(o => o.Value.SourceEvent == eventName)
        //        .Select(o => o.Key)
        //        .ToList();
        //    variables.ForEach(proxy.GetProxy().Variables.Unset);
        //}
    }
}