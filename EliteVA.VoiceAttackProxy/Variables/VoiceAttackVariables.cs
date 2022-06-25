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
            TypeCode code = Convert.GetTypeCode(variable.Value);
            Set(category, variable, code);
        }

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        /// <param name="code">The type of variable</param>
        public void Set(string category, Variable variable, TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Boolean:
                    SetBoolean(category, variable);
                    break;

                case TypeCode.DateTime:
                    SetDate(category, variable);
                    break;

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    SetDecimal(category, variable);
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    SetText(category, variable);
                    break;

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    SetShort(category, variable);
                    break;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    try
                    {
                        SetInt(category, variable);
                    }
                    catch (OverflowException)
                    {
                        SetDecimal(category, variable);
                    }
                    break;

                case TypeCode.Object:
                    var newCode = Convert.GetTypeCode(variable.Value);
                    Set(category, variable, newCode);
                    break;

                case TypeCode.Empty:
                    break;
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

        private short? GetShort(string name)
        {
            return _proxy.GetSmallInt(name);
        }

        private int? GetInt(string name)
        {
            return _proxy.GetInt(name);
        }

        private string GetText(string name)
        {
            return _proxy.GetText(name);
        }

        private decimal? GetDecimal(string name)
        {
            return _proxy.GetDecimal(name);
        }

        private bool? GetBoolean(string name)
        {
            return _proxy.GetBoolean(name);
        }

        private DateTime? GetDate(string name)
        {
            return _proxy.GetDate(name);
        }

        private void SetShort(string category, Variable variable)
        {
            var value = (short)Convert.ChangeType(variable.Value, typeof(short));
            _proxy.SetSmallInt(variable.Name, value);
            variable.Name = $"{{SHORT:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetInt(string category, Variable variable)
        {
            var value = (int)Convert.ChangeType(variable.Value, typeof(int));
            _proxy.SetInt(variable.Name, value);
            variable.Name = $"{{INT:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetText(string category, Variable variable)
        {
            var value = (string)Convert.ChangeType(variable.Value, typeof(string));
            _proxy.SetText(variable.Name, value);
            variable.Name = $"{{TXT:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetDecimal(string category, Variable variable)
        {
            var value = (decimal)Convert.ChangeType(variable.Value, typeof(decimal));
            _proxy.SetDecimal(variable.Name, value);
            variable.Name = $"{{DEC:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetBoolean(string category, Variable variable)
        {
            var value = (bool)Convert.ChangeType(variable.Value, typeof(bool));
            _proxy.SetBoolean(variable.Name, value);
            variable.Name = $"{{BOOL:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetDate(string category, Variable variable)
        {
            var value = (DateTime)Convert.ChangeType(variable.Value, typeof(DateTime));
            _proxy.SetDate(variable.Name, value);
            variable.Name = $"{{DATE:{variable.Name}}}";
            SetVariable(category, variable);
        }

        private void SetVariable(string category, Variable variable)
        {
            _logger.LogTrace("Setting '{Name}' to '{Value}'", variable.Name, variable.Value);
            _setVariables[(category, variable.Name)] = variable;
        }
    }
}