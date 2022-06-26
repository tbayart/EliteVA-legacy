using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class VoiceAttackVariables
    {
        #region fields
        private readonly dynamic _proxy;
        private readonly Dictionary<(string category, string name), Variable> _setVariables;
        private readonly ILogger _logger;
        #endregion fields

        #region ctor
        internal VoiceAttackVariables(dynamic vaProxy, IServiceProvider services)
        {
            _proxy = vaProxy;
            _logger = services.GetService<ILogger<VoiceAttackVariables>>();
            _setVariables = new Dictionary<(string, string), Variable>(10000);
        }
        #endregion ctor

        public IReadOnlyDictionary<(string category, string name), Variable> SetVariables => _setVariables;

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void Set(string category, Variable variable)
        {
            try
            {
                ProxySet(variable.ValueType, variable.Name, variable.Value);
                _setVariables[(category, variable.Name)] = variable;
                _logger.LogTrace("Variable '{Name}' set to '{Value}'", variable.Name, variable.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not set variable {name} to {value}", variable.Name, variable.Value);
            }
        }

        public void Set(string category, IEnumerable<Variable> variables)
        {
            var sourceEvent = variables.Select(o => o.SourceEvent).First();
            if (string.IsNullOrEmpty(sourceEvent) == false)
            {
                // get expired variables to unset
                var toUnset = _setVariables
                    .Where(o => o.Key.category == category)
                    .Select(o => o.Value)
                    .Where(o => o.SourceEvent == sourceEvent)
                    .Except(variables);
                foreach (var variable in toUnset)
                    UnSet(category, variable);
            }
            foreach (var variable in variables)
                Set(category, variable);
        }

        public void UnSet(string category, Variable variable)
        {
            var key = (category, variable.Name);
            if (_setVariables.TryGetValue(key, out variable) == false)
                return;

            try
            {
                ProxySet(variable.ValueType, variable.Name, null);
                _setVariables.Remove(key);
                _logger.LogTrace("Variable '{Name}' unset", variable.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not unset variable {name}", variable.Name);
            }
        }

        /// <summary>
        /// Get a variable
        /// </summary>
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        public T Get<T>(string name)
        {
            TypeCode code = Type.GetTypeCode(typeof(T));

            switch (code)
            {
                case TypeCode.Boolean:
                    return (T)Convert.ChangeType(GetBoolean(name), typeof(T));

                case TypeCode.DateTime:
                    return (T)Convert.ChangeType(GetDate(name), typeof(T));

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return (T)Convert.ChangeType(GetDecimal(name), typeof(T));

                case TypeCode.Char:
                case TypeCode.String:
                    return (T)Convert.ChangeType(GetText(name), typeof(T));

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    return (T)Convert.ChangeType(GetShort(name), typeof(T));

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return (T)Convert.ChangeType(GetInt(name), typeof(T));

                default:
                    return default;
            }
        }

        private void ProxySet(VariableValueType valueType, string name, object value)
        {
            switch (valueType)
            {
                case VariableValueType.SHORT: SetShort(name, (short?)value); break;
                case VariableValueType.INT: SetInt(name, (int?)value); break;
                case VariableValueType.TXT: SetText(name, (string)value); break;
                case VariableValueType.DEC: SetDecimal(name, (decimal?)value); break;
                case VariableValueType.BOOL: SetBoolean(name, (bool?)value); break;
                case VariableValueType.DATE: SetDate(name, (DateTime?)value); break;
            }
        }

        private short? GetShort(string name) => _proxy.GetSmallInt(name);

        private int? GetInt(string name) => _proxy.GetInt(name);

        private string GetText(string name) => _proxy.GetText(name);

        private decimal? GetDecimal(string name) => _proxy.GetDecimal(name);

        private bool? GetBoolean(string name) => _proxy.GetBoolean(name);

        private DateTime? GetDate(string name) => _proxy.GetDate(name);

        private void SetShort(string name, short? value) => _proxy.SetSmallInt(name, value);

        private void SetInt(string name, int? value) => _proxy.SetInt(name, value);

        private void SetText(string name, string value) => _proxy.SetText(name, value);

        private void SetDecimal(string name, decimal? value) => _proxy.SetDecimal(name, value);

        private void SetBoolean(string name, bool? value) => _proxy.SetBoolean(name, value);

        private void SetDate(string name, DateTime? value) => _proxy.SetDate(name, value);
    }
}