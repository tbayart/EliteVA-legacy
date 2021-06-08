using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Plugin.Services.Variable.Abstractions
{
    public interface IVariableService
    {
        /// <summary>
        /// Sets a VoiceAttack variable
        /// </summary>
        /// <param name="variable">The variable to be set</param>
        void SetVariable(string category, Plugin.Variable variable);

        /// <summary>
        /// Gets a VoiceAttack variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        T GetVariable<T>(string name);
        
        /// <summary>
        /// Gets all JToken's from a JObject
        /// </summary>
        List<Plugin.Variable> GetPaths(JObject jObject);
        
        /// <summary>
        /// Gets all JToken's from a JArray
        /// </summary>
        List<Plugin.Variable> GetPaths(JArray jArray);
    }
}