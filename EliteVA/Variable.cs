namespace EliteVA
{
    public readonly struct Variable
    {
        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public readonly string Name;
        public readonly object Value;
    }
}