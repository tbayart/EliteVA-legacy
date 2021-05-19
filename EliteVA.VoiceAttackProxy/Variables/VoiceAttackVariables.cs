using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class VoiceAttackVariables
    {
        private readonly dynamic _proxy;

        private Dictionary<string, string> _setVariables;

        public IReadOnlyDictionary<string, string> SetVariables => _setVariables;

        public ILogger<VoiceAttackVariables> _log;
        
        internal VoiceAttackVariables(dynamic vaProxy, IServiceProvider services)
        {
            _proxy = vaProxy;
            _log = services.GetService<ILogger<VoiceAttackVariables>>();

            _setVariables = new Dictionary<string, string>();
        }

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <typeparam name="T">The type of variable</typeparam>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        public void Set<T>(string name, T value)
        {
            TypeCode code = Convert.GetTypeCode(value);
            Set(name, value, code);
        }

        /// <summary>
        /// Set a variable
        /// </summary>
        /// <param name="name">The name of the variable</param>
        /// <param name="value">The value of the variable</param>
        /// <param name="code">The type of variable</param>
        public void Set(string name, object value, TypeCode code)
        {
            switch (code)
            {
                case TypeCode.Boolean:
                    SetBoolean(name, (bool) Convert.ChangeType(value, typeof(bool)));
                    break;

                case TypeCode.DateTime:
                    SetDate(name, (DateTime) Convert.ChangeType(value, typeof(DateTime)));
                    break;

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                    SetDecimal(name, (decimal) Convert.ChangeType(value, typeof(decimal)));
                    break;

                case TypeCode.Char:
                case TypeCode.String:
                    SetText(name, (string) Convert.ChangeType(value, typeof(string)));
                    break;

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    SetShort(name, (short) Convert.ChangeType(value, typeof(short)));
                    break;

                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    try
                    {
                        SetInt(name, (int) Convert.ChangeType(value, typeof(int)));
                    }
                    catch (OverflowException ex)
                    {
                        SetDecimal(name, (decimal) Convert.ChangeType(value, typeof(decimal)));
                    } 
                    break;

                case TypeCode.Object:
                    var newCode = Convert.GetTypeCode(value);
                    Set(name, value, newCode);
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
                    return (T) Convert.ChangeType(GetBoolean(name), typeof(T));

                case TypeCode.DateTime:
                    return (T) Convert.ChangeType(GetDate(name), typeof(T));

                case TypeCode.Single:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return (T) Convert.ChangeType(GetDecimal(name), typeof(T));

                case TypeCode.Char:
                case TypeCode.String:
                    return (T) Convert.ChangeType(GetText(name), typeof(T));

                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.SByte:
                    return (T) Convert.ChangeType(GetShort(name), typeof(T));

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return (T) Convert.ChangeType(GetInt(name), typeof(T));

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

        private void SetShort(string name, short? value)
        {
            string variable = $"{{SHORT:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetSmallInt(name, value);
        }

        private void SetInt(string name, int? value)
        {
            string variable = $"{{INT:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetInt(name, value);
        }

        private void SetText(string name, string value)
        {
            string variable = $"{{TXT:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetText(name, value);
        }

        private void SetDecimal(string name, decimal? value)
        {
            string variable = $"{{DEC:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetDecimal(name, value);
        }

        private void SetBoolean(string name, bool? value)
        {
            string variable = $"{{BOOL:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetBoolean(name, value);
        }

        private void SetDate(string name, DateTime? value)
        {
            string variable = $"{{DATE:{name}}}";
            SetVariable(variable, value.ToString());

            _proxy.SetDate(name, value);
        }

        private void SetVariable(string name, string value)
        {
            _log.LogTrace("Setting '{Name}' to '{Value}'", name, value);
            
            if (_setVariables.ContainsKey(name)) { _setVariables[name] = value; }
            else { _setVariables.Add(name, value); }
        }
    }
}