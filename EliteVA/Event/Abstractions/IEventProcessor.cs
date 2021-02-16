using System.Collections.Generic;

using EliteAPI.Event.Models.Abstractions;

namespace EliteVA.Event.Abstractions
{
    public interface IEventProcessor
    {
        /// <summary>
        /// Binds all the events to VoiceAttack commands
        /// </summary>
        void Bind();

        /// <summary>
        /// Generates the event's properties as VoiceAttack variables
        /// </summary>
        /// <param name="e"></param>
        IEnumerable<Variable> GetVariables(IEvent e);
        
        /// <summary>
        /// Generates a VoiceAttack event command
        /// </summary>
        /// <param name="e">The event</param>
        string GetCommand(IEvent e);
    }
}