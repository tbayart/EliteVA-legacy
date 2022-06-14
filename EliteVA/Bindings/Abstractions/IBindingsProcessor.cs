using EliteVA.VoiceAttackProxy.Variables;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EliteVA.Bindings.Abstractions
{
    public interface IBindingsProcessor
    {
        /// <summary>
        /// Binds all the keybindings to VoiceAttack variables
        /// </summary>
        void Bind();

        /// <summary>
        /// Gets the keyboard layout of a bindings xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        string GetLayout(XElement xml);
        
        /// <summary>
        /// Gets the mappings from the configuration direction
        /// </summary>
        /// <param name="layout"></param>
        /// <returns></returns>
        IDictionary<string, string> GetMapping(string layout);
        
        /// <summary>
        /// Generates the variables needed
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="mapping"></param>
        /// <returns></returns>
        IEnumerable<Variable> GetVariables(XElement xml, IDictionary<string, string> mapping);
    }
}