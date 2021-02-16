using System.Collections.Generic;

using EliteAPI.Options.Bindings.Models;

namespace EliteVA.Bindings.Abstractions
{
    public interface IBindingsProcessor
    {
        /// <summary>
        /// Binds all the keybindings to VoiceAttack variables
        /// </summary>
        void Bind();
        
        /// <summary>
        /// Reads the layout for the specified keyboard layout
        /// </summary>
        /// <param name="mappingsPath">The path to the mappings folder</param>
        /// <param name="keyboardLayout">The name of the keyboard layout</param>
        /// <returns></returns>
        IDictionary<string, string> ReadLayout(string mappingsPath, string keyboardLayout);
        
        /// <summary>
        /// Gets all the variables for a specific keyboard layout
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        IEnumerable<Variable> GetVariables(KeyBindings bindings, IDictionary<string, string> layout);
    }
}