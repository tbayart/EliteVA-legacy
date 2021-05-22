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
        IEnumerable<Variable> GetVariables(string eventName, string json);
        
        /// <summary>
        /// Generates a VoiceAttack event command
        /// </summary>
        /// <param name="e">The event</param>
        string GetCommand(IEvent e);
    }
}