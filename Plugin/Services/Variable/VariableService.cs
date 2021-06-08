using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Plugin.Constants.Paths.Abstractions;
using Plugin.Services.Variable.Abstractions;

namespace Plugin.Services.Variable
{
    public abstract class VariableService : IVariableService
    {
        private readonly ILogger<VariableService> logger;
        private readonly IPaths paths;

        protected VariableService(ILogger<VariableService> logger, IPaths paths)
        {
            this.logger = logger;
            this.paths = paths;
        }
        
        /// <inheritdoc />
        public abstract T GetVariable<T>(string name);

        /// <inheritdoc />
        public abstract void SetVariable(string category, Plugin.Variable variable);

        // {
        //     try
        //     {
        //         proxy.GetProxy().Variables.Set(category, variable.Name, variable.Value); 
        //     }
        //     catch (Exception ex)
        //     {
        //         logger.LogDebug(ex, "Could not set variable {name} to {value}", variable.Name, variable.Value);
        //     }
        // }
        
        public List<Plugin.Variable> GetPaths(JObject jObject)
        {
            try
            {
                return jObject.Properties().SelectMany(GetPaths).ToList();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot get event variable paths (object)");
                return new List<Plugin.Variable>();
            }
        }

        public List<Plugin.Variable> GetPaths(JArray jArray)
        {
            try
            {
                return jArray.Values<JObject>().SelectMany(x => x.Properties().SelectMany(GetPaths)).ToList();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Cannot get event variable paths (array)");
                return new List<Plugin.Variable>();
            }
        }

        private List<Plugin.Variable> GetPaths(JProperty property)
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
                        return new List<Plugin.Variable>
                            {new Plugin.Variable(property.Value.Path, property.Value.ToObject<bool>())};

                    case JTokenType.String:
                        return new List<Plugin.Variable>
                            {new Plugin.Variable(property.Value.Path, property.Value.ToObject<string>())};

                    case JTokenType.Date:
                        return new List<Plugin.Variable>
                            {new Plugin.Variable(property.Value.Path, property.Value.ToObject<DateTime>())};

                    case JTokenType.Integer:
                        try
                        {
                            return new List<Plugin.Variable>
                                {new Plugin.Variable(property.Value.Path, property.Value.ToObject<int>())};
                        }
                        catch (OverflowException)
                        {
                            return new List<Plugin.Variable>
                                {new Plugin.Variable(property.Value.Path, property.Value.ToObject<long>())};
                        }

                    case JTokenType.Float:
                        return new List<Plugin.Variable>
                            {new Plugin.Variable(property.Value.Path, property.Value.ToObject<decimal>())};

                    default:
                        return new List<Plugin.Variable>();
                }
            }
            catch (InvalidCastException ex)
            {
                logger.LogDebug(ex, "Could not process {Path}", property.Value.Path);
                return new List<Plugin.Variable>();
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not process {Path}", property.Value.Path);
                return new List<Plugin.Variable>();
            }
        }
    }
}