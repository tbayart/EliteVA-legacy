using System;

namespace EliteVA.VoiceAttackProxy.Variables
{
    public class Variable
    {
        public Variable(string sourceEvent, string name, object value)
        {
            SourceEvent = sourceEvent;
            Name = name;
            Value = value;
        }

        public string SourceEvent { get; }

        public string Name { get; set; }

        public object Value { get; }
    }
}