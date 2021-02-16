using System.Collections.Generic;

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
    }
}