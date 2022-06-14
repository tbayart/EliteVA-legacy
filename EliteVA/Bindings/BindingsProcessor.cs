using EliteAPI.Options.Processor.Abstractions;
using EliteVA.Bindings.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EliteVA.Bindings
{
    public class BindingsProcessor : IBindingsProcessor
    {
        private readonly ILogger<BindingsProcessor> log;
        private readonly IOptionsProcessor optionsProcessor;
        private readonly IFormatting formats;
        private readonly IVariableService variables;
        private readonly IPaths paths;

        public BindingsProcessor(ILogger<BindingsProcessor> log, IOptionsProcessor optionsProcessor,
            IFormatting formats, IVariableService variables, IPaths paths)
        {
            this.log = log;
            this.optionsProcessor = optionsProcessor;
            this.formats = formats;
            this.variables = variables;
            this.paths = paths;
        }

        /// <inheritdoc />
        public void Bind()
        {
            optionsProcessor.BindingsUpdated += (sender, e) =>
            {
                try
                {
                    log.LogDebug("Updating bindings ...");
                    var xml = XElement.Parse(e);

                    var layout = GetLayout(xml);
                    var mapping = GetMapping(layout);
                    var keys = GetVariables(xml, mapping).ToList();

                    variables.SetVariables("Bindings", keys);
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Could not set keybindings");
                }
            };
        }

        public string GetLayout(XElement xml)
        {
            try
            {
                var layoutElement = xml.Element("KeyboardLayout");
                return layoutElement != null ? layoutElement.Value : "en-US";
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Could not get layout from keybindings");
                throw;
            }
        }

        public IDictionary<string, string> GetMapping(string layout)
        {
            try
            {
                string mappingFile = Path.Combine(paths.MappingsDirectory.FullName, $"{layout}.yml");

                if (!File.Exists(mappingFile))
                {
                    if (layout != "en-GB")
                    {
                        log.LogWarning(
                            "Unsupported keybindings layout, could not find the {Layout}.yml mapping, defaulting to en-GB.yml",
                            layout);
                        return GetMapping("en-GB");
                    }

                    log.LogError("Could not set keybindings, no mappings found");
                    return new Dictionary<string, string>();
                }

                log.LogDebug("Reading '{MappingPath}'", mappingFile);

                var entries = File.ReadAllLines(mappingFile)
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains(":"));
                return entries.ToDictionary(entry => entry.Split(':')[0].Trim(), entry => entry.Split(':')[1].Trim());
            }
            catch (Exception ex)
            {
                log.LogWarning(ex, "Could not get mappings from {Layout},yml", layout);
                throw;
            }
        }

        public IEnumerable<Variable> GetVariables(XElement xml, IDictionary<string, string> mapping)
        {
            IList<Variable> variables = new List<Variable>();

            foreach (var bindingNode in xml.Elements().Where(i => i.Elements().Any()))
            {
                try
                {
                    var name = bindingNode.Name.LocalName;

                    var primary = bindingNode.Element("Primary");
                    var secondary = bindingNode.Element("Secondary");

                    if (primary == null)
                    {
                        log.LogDebug("Skipping {Name}, no bindings set", name);
                        continue;
                    }

                    XElement active = null;

                    if (IsApplicableBinding(primary))
                    {
                        active = primary;
                    }
                    else if (IsApplicableBinding(secondary))
                    {
                        active = secondary;
                    }
                    else
                    {
                        log.LogDebug("Skipping {Name}, not applicable", name);
                        continue;
                    }

                    if (active == null) continue;

                    var modifiers = active.Elements("Modifier").ToList();

                    if (modifiers.Any(x => !IsApplicableBinding(x)))
                    {
                        log.LogDebug("Skipping {Name}, modifier not applicable", name);
                    }

                    string value = GetKeyBinding(active.Attribute("Key").Value, modifiers.Select(x => x.Attribute("Key").Value), mapping);

                    variables.Add(new Variable($"EliteAPI.{name}", value));
                }
                catch (Exception ex)
                {
                    log.LogWarning(ex, "Could not process {Name} keybinding", bindingNode.Name);
                }
            }

            return variables;
        }

        private string GetKeyBinding(string key, IEnumerable<string> mods, IDictionary<string, string> mapping)
        {
            return string.Join("", mods.Select(mod => GetKey(mod, mapping))) + GetKey(key, mapping);
        }

        private string GetKey(string key, IDictionary<string, string> mapping)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return key;
            }

            key = key.Replace("Key_", "");

            if (!mapping.ContainsKey(key))
            {
                log.LogDebug("The '{Key}' key is not assigned in the mappings file", key);
                return "";
            }

            return $"[{mapping[key]}]";
        }

        private bool IsApplicableBinding(XElement xml)
        {
            var deviceNode = xml.Attribute("Device");
            var keyNode = xml.Attribute("Key");

            return deviceNode != null && deviceNode.Value == "Keyboard" && keyNode != null &&
                   keyNode.Value.StartsWith("Key_");
        }
    }
}