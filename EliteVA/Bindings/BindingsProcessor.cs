using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using EliteAPI.Abstractions;
using EliteAPI.Event.Models.Abstractions;
using EliteAPI.Options.Bindings.Models;

using EliteVA.Bindings.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Services;
using EliteVA.Services.Variable;

using Microsoft.Extensions.Logging;

namespace EliteVA.Bindings
{
    public class BindingsProcessor : IBindingsProcessor
    {
        private readonly ILogger<BindingsProcessor> log;
        private readonly IEliteDangerousApi api;
        private readonly IFormatting formats;
        private readonly IVariableService variables;
        private readonly IPaths paths;

        public BindingsProcessor(ILogger<BindingsProcessor> log, IEliteDangerousApi api, IFormatting formats, IVariableService variables, IPaths paths)
        {
            this.log = log;
            this.api = api;
            this.formats = formats;
            this.variables = variables;
            this.paths = paths;
        }

        /// <inheritdoc />
        public void Bind()
        {
            api.Bindings.OnChange += (sender, e) =>
            {
                var layout = ReadLayout(paths.MappingsDirectory.FullName, api.Bindings.Active.KeyboardLayout);

                if (layout == default)
                {
                    log.LogWarning("Cannot set keybindings. Could not find {Layout}.yml or en-GB.yml in {Bindings}", api.Bindings.Active.KeyboardLayout, paths.MappingsDirectory.FullName);
                }
                else
                {
                    var variable = GetVariables(api.Bindings.Active, layout);
                    variables.SetVariables("Bindings", variable);
                }
            };
        }

        /// <inheritdoc />
        public IDictionary<string, string> ReadLayout(string mappingsPath, string keyboardLayout)
        {
            var layoutFile = new FileInfo(Path.Combine(mappingsPath, $"{keyboardLayout}.yml"));

            log.LogDebug("Reading layout {Path}", layoutFile.FullName);
            
            // If the bindings file doesn't exist and we haven't tried en-GB, try en-GB
            if (!layoutFile.Exists) { return keyboardLayout != "en-GB" ? ReadLayout(mappingsPath, "en-GB") : default; }

            var entries = File.ReadAllLines(layoutFile.FullName).Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains(":"));
            return entries.ToDictionary(entry => entry.Split(':')[0].Trim(), entry => entry.Split(':')[1].Trim());
        }

        /// <inheritdoc />
        public IEnumerable<Variable> GetVariables(KeyBindings bindings, IDictionary<string, string> layout)
        {
            return bindings.GetType().GetProperties().Select(x => GetVariable(x, api.Bindings.Active, layout)).Where(x => !string.IsNullOrWhiteSpace(x.Name));
        }

        private Variable GetVariable(PropertyInfo property, object instance, IDictionary<string, string> layout)
        {
            object value = property.GetValue(instance);

            try
            {
                var test1 = ((dynamic) value).Primary;
                var test2 = ((dynamic) value).Secondary;
            }
            catch
            {
                // Is not applicable, skip
                return default;
            }

            KeyBindings.PrimaryBinding primary = ((dynamic) value).Primary;
            KeyBindings.SecondaryBinding secondary = ((dynamic) value).Secondary;

            if (string.IsNullOrWhiteSpace(primary.Key) && string.IsNullOrWhiteSpace(secondary.Key))
            {
                // Not in use, skip
                return default;
            }

            string key = primary.Key;
            KeyBindings.ModifierBinding modifier = primary.Modifier;

            if (primary.Device != "Keyboard")
            {
                if (secondary.Device == "Keyboard")
                {
                    key = secondary.Key;
                    modifier = secondary.Modifier;
                }
                else
                {
                    // No keyboard bindings, skip
                    return default;
                }
            }

            string mod = modifier != null ? modifier.Key : "";
            string name = formats.Bindings.ToVariable(property.Name);
            string keyValue = GetKeyBinding(key, mod, layout);

            return new Variable(name, keyValue);
        }

        private string GetKeyBinding(string key, string mod, IDictionary<string, string> layout)
        {
            key = GetKey(key, layout);
            mod = GetKey(mod, layout);

            return mod + key;
        }

        private string GetKey(string key, IDictionary<string, string> layout)
        {
            if (string.IsNullOrWhiteSpace(key)) { return key; }

            key = key.Replace("Key_", "");

            key = layout.ContainsKey(key) ? layout[key] : "";

            return $"[{key}]";
        }
    }
}