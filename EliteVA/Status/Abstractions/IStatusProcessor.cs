using System.Collections.Generic;

namespace EliteVA.Status.Abstractions
{
    public interface IStatusProcessor
    {
        /// <summary>
        /// Binds all the Ship events to VoiceAttack commands
        /// </summary>
        void Bind();
        
        /// <summary>
        /// Generates a VoiceAttack ship variable
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        IEnumerable<Variable> GetVariables(string name, object value);
        
        /// <summary>
        /// Generates a VoiceAttack ship command
        /// </summary>
        /// <param name="name">The name of the event</param>
        string GetCommand(string name);
    }
}