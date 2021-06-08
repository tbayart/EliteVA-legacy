using System;
using Microsoft.Extensions.Logging;
using Plugin;
using Plugin.Constants.Paths.Abstractions;

namespace EliteVA.Variables
{
    public class VariableService : Plugin.Services.Variable.VariableService
    {
        public VariableService(ILogger<Plugin.Services.Variable.VariableService> logger, IPaths paths) : base(logger, paths)
        {
        }

        public override T GetVariable<T>(string name) =>
            VoiceAttackPlugin.Proxy.Variables.Get<T>(name);

        public override void SetVariable(string category, Variable variable) =>
            VoiceAttackPlugin.Proxy.Variables.Set(category, variable.Name, variable.Value);
    }
}