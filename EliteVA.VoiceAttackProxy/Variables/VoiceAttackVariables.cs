using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void Set(string category, Variable variable)
        {
            try
            {
                switch (variable.ValueType)
                {
                    case VariableValueType.BOOL: SetBoolean(variable); break;
                    case VariableValueType.DATE: SetDate(variable); break;
                    case VariableValueType.DEC: SetDecimal(variable); break;
                    case VariableValueType.TXT: SetText(variable); break;
                    case VariableValueType.SHORT: SetShort(variable); break;
                    case VariableValueType.INT: SetInt(variable); break;
                }
            }
            catch (InvalidCastException ex)
            {
                _ = ex;
            }
            catch (Exception ex)
            {
                _ = ex;
            }
            SetVariable(category, variable);
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

        private short? GetShort(string name) => _proxy.GetSmallInt(name);

        private int? GetInt(string name) => _proxy.GetInt(name);

        private string GetText(string name) => _proxy.GetText(name);

        private decimal? GetDecimal(string name) => _proxy.GetDecimal(name);

        private bool? GetBoolean(string name) => _proxy.GetBoolean(name);

        private DateTime? GetDate(string name) => _proxy.GetDate(name);

        private void SetShort(Variable variable) => _proxy.SetSmallInt(variable.Name, (short)variable.Value);

        private void SetInt(Variable variable) => _proxy.SetInt(variable.Name, (int)variable.Value);

        private void SetText(Variable variable) => _proxy.SetText(variable.Name, (string)variable.Value);

        private void SetDecimal(Variable variable) => _proxy.SetDecimal(variable.Name, (decimal)variable.Value);

        private void SetBoolean(Variable variable) => _proxy.SetBoolean(variable.Name, (bool)variable.Value);

        private void SetDate(Variable variable) => _proxy.SetDate(variable.Name, (DateTime)variable.Value);

        private void SetVariable(string category, Variable variable)
        {
            _logger.LogTrace("Setting '{Name}' to '{Value}'", variable.Name, variable.Value);
            _setVariables[(category, variable.Name)] = variable;
        }
    }
}