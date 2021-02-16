using System;
using System.IO;
using System.Reflection;

using EliteVA.Constants.Paths.Abstractions;

namespace EliteVA.Constants.Paths
{
    public class Paths : IPaths
    {
        public Paths()
        {
            PluginDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? AppDomain.CurrentDomain.BaseDirectory);
            MappingsDirectory = new DirectoryInfo(Path.Combine(PluginDirectory.FullName, "Mappings"));
        }
        
        public DirectoryInfo PluginDirectory { get; private set; }
        
        public DirectoryInfo MappingsDirectory { get; private set; }
    }
}