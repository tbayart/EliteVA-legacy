using System;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class Variable
    {
        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public object Value { get; }
    }
}