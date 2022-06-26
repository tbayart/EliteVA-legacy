using EliteVA.VoiceAttackProxy.Variables;
using System.Collections.Generic;

namespace EliteVA.Support.Abstractions
{
    public interface ISupportProcessor
    {
        /// <summary>
        /// Binds all the Supports events to VoiceAttack commands
        /// </summary>
        void Bind();
        
        /// <summary>
        /// Generates a VoiceAttack support variable
        /// </summary>
        /// <param name="name">The name of the property</param>
        /// <param name="value">The value of the property</param>
        IEnumerable<Variable> GetVariables(string name, object value);
        
        /// <summary>
        /// Generates a VoiceAttack support command
        /// </summary>
        /// <param name="name">The name of the event</param>
        string GetCommand(string name);
    }
}
