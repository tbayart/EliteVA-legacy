using EliteVA.VoiceAttackProxy.Variables;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EliteVA.Services
{
    public interface IVariableService
    {
        /// <summary>
        /// Sets a VoiceAttack variable
        /// </summary>
        /// <param name="variable">The variable to be set</param>
        void SetVariable(string category, Variable variable);
        
        /// <summary>
        /// Sets VoiceAttack variables
        /// </summary>
        /// <param name="variables">The variables to be set</param>
        void SetVariables(string category, IEnumerable<Variable> variables);

        /// <summary>
        /// Gets all JToken's from a JObject
        /// </summary>
        List<Variable> GetPaths(JObject jObject);
        
        /// <summary>
        /// Gets all JToken's from a JArray
        /// </summary>
        List<Variable> GetPaths(JArray jArray);
    }
}