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
        private readonly string _variablesPath;
        #endregion fields

        #region ctor
        public VariableService(ILogger<VariableService> logger, IProxy proxy, IPaths paths)
        {
            _logger = logger;
            _proxy = proxy;
            _variablesPath = Path.Combine(paths.PluginDirectory.FullName, "Variables");
            Directory.CreateDirectory(_variablesPath);
        }
        #endregion ctor

        #region IVariableService
        /// <inheritdoc />
        public void SetVariables(string category, IEnumerable<Variable> variables)
        {
            var vaVariables = _proxy.GetProxy().Variables;
            try
            {
                vaVariables.Set(category, variables);
                var setVariables = vaVariables.SetVariables
                    .Where(x => x.Key.category == category)
                    .Select(o => o.Value.ToString())
                    .ToList();
                var variablesPath = Path.Combine(_variablesPath, category + ".txt");
                File.WriteAllLines(variablesPath, setVariables);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Cannot save variables for [{Category}]", category);
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
        #endregion IVariableService

        #region methods
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
                }
            }
            catch (InvalidCastException ex)
            {
                _logger.LogDebug(ex, "Could not process {Path}", property.Value.Path);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Could not process {Path}", property.Value.Path);
            }
            return new List<Variable>();
        }
        #endregion methods
    }
}
