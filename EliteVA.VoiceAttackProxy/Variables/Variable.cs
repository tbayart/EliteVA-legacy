using System;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class Variable
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

        public override string ToString() => _toString;
    }

    public enum VariableValueType { BOOL, DATE, DEC, TXT, SHORT, INT }

    public static class VariableHelpers
    {
        public static VariableValueType ResolveValueType(ref object value)
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
                    return VariableValueType.INT;
            }

            value = value?.ToString();
            return VariableValueType.TXT;
        }
    }
}
