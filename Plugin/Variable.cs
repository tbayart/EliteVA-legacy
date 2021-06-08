using System;

namespace Plugin
{
    public class Variable
    {
        public Variable(string name, string value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, int value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, long value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, decimal value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, DateTime value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, bool value)
        {
            Name = name;
            Value = value;
        }
        
        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; }
    }
}