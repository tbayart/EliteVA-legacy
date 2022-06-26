using System;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class Variable : IEquatable<Variable>
    {
        private string _toString;

        public Variable(string sourceEvent, string name, object value)
        {
            SourceEvent = sourceEvent;
            Name = name;
            ValueType = VariableHelpers.ResolveValueType(ref value);
            Value = value;
            _toString = $"{{{ValueType}:{Name}}}: {Value}";
        }

        public string SourceEvent { get; }

        public string Name { get; }

        public object Value { get; }

        public VariableValueType ValueType { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString() => _toString;

        public override bool Equals(object obj)
        {
            return Equals(obj as Variable);
        }

        public bool Equals(Variable other)
        {
            return other?.Name == Name;
        }
    }

    public enum VariableValueType { BOOL, DATE, DEC, TXT, SHORT, INT }

    public static class VariableHelpers
    {
        public static VariableValueType ResolveValueType(ref object value)
        {
            try
            {
                switch (Convert.GetTypeCode(value))
                {
                    case TypeCode.Boolean:
                        return VariableValueType.BOOL;

                    case TypeCode.DateTime:
                        return VariableValueType.DATE;

                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        value = Convert.ChangeType(value, TypeCode.Decimal);
                        return VariableValueType.DEC;

                    case TypeCode.String:
                        return VariableValueType.TXT;

                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.SByte:
                        value = Convert.ChangeType(value, TypeCode.Int16);
                        return VariableValueType.SHORT;

                    case TypeCode.Int32:
                        value = Convert.ChangeType(value, TypeCode.Int32);
                        return VariableValueType.INT;
                }
            }
            catch (Exception ex)
            {
                _ = ex;
            }
            value = value?.ToString();
            return VariableValueType.TXT;
        }
    }
}
