using EliteAPI.Options.Processor.Abstractions;
using EliteVA.Bindings.Abstractions;
using EliteVA.Constants.Formatting.Abstractions;
using EliteVA.Constants.Paths.Abstractions;
using EliteVA.Constants.Proxy.Abstractions;
using EliteVA.Services;
using EliteVA.VoiceAttackProxy.Variables;
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
        #region fields
        private readonly ILogger _logger;
        private readonly IOptionsProcessor _optionsProcessor;
        private readonly IVariableService _variables;
        private readonly IPaths _paths;
        #endregion fields

        #region ctor
        public BindingsProcessor(ILogger<BindingsProcessor> log, IOptionsProcessor optionsProcessor,
            IVariableService variables, IPaths paths)
        {
            _logger = log;
            _optionsProcessor = optionsProcessor;
            _variables = variables;
            _paths = paths;
        }
        #endregion ctor

        /// <inheritdoc />
        public void Bind()
        {
            _optionsProcessor.BindingsUpdated += (sender, e) =>
            {
                try
                {
                    _logger.LogDebug("Updating bindings ...");
                    var xml = XElement.Parse(e);

                    var layout = GetLayout(xml);
                    var mapping = GetMapping(layout);
                    var keys = GetVariables(xml, mapping).ToList();

                    _variables.SetVariables("Bindings", keys);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not set keybindings");
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
                _logger.LogError(ex, "Could not get layout from keybindings");
                throw;
            }
        }

        public IDictionary<string, string> GetMapping(string layout)
        {
            try
            {
                string mappingFile = Path.Combine(_paths.MappingsDirectory.FullName, $"{layout}.yml");

                if (!File.Exists(mappingFile))
                {
                    if (layout != "en-GB")
                    {
                        _logger.LogWarning(
                            "Unsupported keybindings layout, could not find the {Layout}.yml mapping, defaulting to en-GB.yml",
                            layout);
                        return GetMapping("en-GB");
                    }

                    _logger.LogError("Could not set keybindings, no mappings found");
                    return new Dictionary<string, string>();
                }

                _logger.LogDebug("Reading '{MappingPath}'", mappingFile);

                var entries = File.ReadAllLines(mappingFile)
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x.Contains(":"));
                return entries.ToDictionary(entry => entry.Split(':')[0].Trim(), entry => entry.Split(':')[1].Trim());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not get mappings from {Layout},yml", layout);
                throw;
            }
        }

        public IEnumerable<Variable> GetVariables(XElement xml, IDictionary<string, string> mapping)
        {

            var nodes = xml.Elements().Where(i => i.Elements().Any()).ToArray();
            var variables = new List<Variable>(nodes.Length);

            foreach (var bindingNode in nodes)
            {
                try
                {
                    var name = bindingNode.Name.LocalName;
                    var primary = bindingNode.Element("Primary");
                    var secondary = bindingNode.Element("Secondary");

                    if (primary == null)
                    {
                        _logger.LogDebug("Skipping {Name}, no bindings set", name);
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
                        _logger.LogDebug("Skipping {Name}, not applicable", name);
                        continue;
                    }

                    if (active == null) continue;

                    var modifiers = active.Elements("Modifier").ToList();

                    if (modifiers.Any(x => !IsApplicableBinding(x)))
                    {
                        _logger.LogDebug("Skipping {Name}, modifier not applicable", name);
                    }

                    string value = GetKeyBinding(active.Attribute("Key").Value, modifiers.Select(x => x.Attribute("Key").Value), mapping);

                    variables.Add(new Variable(string.Empty, $"EliteAPI.{name}", value));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not process {Name} keybinding", bindingNode.Name);
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
                _logger.LogDebug("The '{Key}' key is not assigned in the mappings file", key);
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