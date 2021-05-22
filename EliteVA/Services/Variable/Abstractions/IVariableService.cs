using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace EliteVA.Services.Variable
{
    public interface IVariableService
    {
        /// <summary>
        /// Sets a VoiceAttack variable
        /// </summary>
        /// <param name="variable">The variable to be set</param>
        void SetVariable(EliteVA.Variable variable);
        
        /// <summary>
        /// Sets VoiceAttack variables
        /// </summary>
        /// <param name="variables">The variables to be set</param>
        void SetVariables(IEnumerable<EliteVA.Variable> variables);

        /// <summary>
        /// Gets all JToken's from a JObject
        /// </summary>
        List<EliteVA.Variable> GetPaths(JObject jObject);
        
        /// <summary>
        /// Gets all JToken's from a JArray
        /// </summary>
        List<EliteVA.Variable> GetPaths(JArray jArray);
    }
}