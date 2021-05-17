using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;

using Microsoft.Extensions.Logging;

namespace EliteVA.Services.Variable
{

    public class VariableService : IVariableService
    {
        private IDictionary<string, string> ActiveVariables { get; }

        private readonly ILogger<VariableService> logger;
        private readonly IProxy proxy;
        private readonly IPaths paths;

        public VariableService(ILogger<VariableService> logger, IProxy proxy, IPaths paths)
        {
            this.logger = logger;
            this.proxy = proxy;
            this.paths = paths;

            ActiveVariables = new Dictionary<string, string>();
        }

        /// <inheritdoc />
        public void SetVariable(EliteVA.Variable variable)
        {
            try
            {
                proxy.GetProxy().Variables.Set(variable.Name, variable.Value);

                string value = variable.Value != null ? variable.Value.ToString() : "";

                if (!ActiveVariables.ContainsKey(variable.Name)) { ActiveVariables.Add(variable.Name, value); }
                else { ActiveVariables[variable.Name] = value; }
                
                File.WriteAllLines(Path.Combine(paths.PluginDirectory.FullName, "ActiveVariables.txt"), proxy.GetProxy().Variables.SetVariables.Select(x => x.Key + ": " + x.Value));
            }
            catch (Exception ex) { logger.LogDebug(ex, "Could not set variable {name} to {value}", variable.Name, variable.Value); }

        }

        /// <inheritdoc />
        public void SetVariables(IEnumerable<EliteVA.Variable> variables)
        {
            foreach (var variable in variables) { SetVariable(variable); }
        }
    }
}