using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace EliteVA.Services.Variable
{
    public class VariableService : IVariableService
    {
        private readonly ILogger<VariableService> logger;
        private readonly IProxy proxy;
        private readonly IPaths paths;

        public VariableService(ILogger<VariableService> logger, IProxy proxy, IPaths paths)
        {
            this.logger = logger;
            this.proxy = proxy;
            this.paths = paths;
        }

        /// <inheritdoc />
        public void SetVariable(EliteVA.Variable variable)
        {
            try
            {
                proxy.GetProxy().Variables.Set(variable.Name, variable.Value);
                
                var allVariables = proxy.GetProxy().Variables.SetVariables.Select(x => x.Key + ": " + x.Value);
                File.WriteAllLines(Path.Combine(paths.PluginDirectory.FullName, "ActiveVariables.txt"), allVariables);
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Could not set variable {name} to {value}", variable.Name, variable.Value);
            }
        }

        /// <inheritdoc />
        public void SetVariables(IEnumerable<EliteVA.Variable> variablesEnumerable)
        {
            try
            {
                var variables = variablesEnumerable.ToList();
                foreach (var variable in variables)
                {
                    SetVariable(variable);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot set variables");
            }
        }

        public List<EliteVA.Variable> GetPaths(JObject jObject)
        {
            try
            {
                return jObject.Properties().SelectMany(GetPaths).ToList();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot get event variable paths (object)");
                return new List<EliteVA.Variable>();
            }
        }

        public List<EliteVA.Variable> GetPaths(JArray jArray)
        {
            try
            {
                return jArray.Values<JObject>().SelectMany(x => x.Properties().SelectMany(GetPaths)).ToList();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot get event variable paths (array)");
                return new List<EliteVA.Variable>();
            }
        }

        private List<EliteVA.Variable> GetPaths(JProperty property)
        {
            try
            {
                switch (property.Value.Type)
                {
                    case JTokenType.Object:
                        return property.Value.Children<JProperty>().SelectMany(GetPaths).ToList();

                    case JTokenType.Array:
                        return property.Value.Values<JObject>().SelectMany(GetPaths).ToList();

                    case JTokenType.Boolean:
                        return new List<EliteVA.Variable>
                            {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<bool>())};

                    case JTokenType.String:
                        return new List<EliteVA.Variable>
                            {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<string>())};

                    case JTokenType.Date:
                        return new List<EliteVA.Variable>
                            {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<DateTime>())};

                    case JTokenType.Integer:
                        try
                        {
                            return new List<EliteVA.Variable>
                                {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<int>())};
                        }
                        catch (OverflowException)
                        {
                            return new List<EliteVA.Variable>
                                {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<long>())};
                        }

                    case JTokenType.Float:
                        return new List<EliteVA.Variable>
                            {new EliteVA.Variable(property.Value.Path, property.Value.ToObject<decimal>())};

                    default:
                        return new List<EliteVA.Variable>();
                }
            }
            catch (InvalidCastException ex)
            {
                logger.LogDebug(ex, "Could not process {Path}", property.Value.Path);
                return new List<EliteVA.Variable>();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not process {Path}", property.Value.Path);
                return new List<EliteVA.Variable>();
            }
        }
    }
}