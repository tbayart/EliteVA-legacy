using System.IO;

namespace EliteVA.Constants.Paths.Abstractions
{
    public interface IPaths
    {
        DirectoryInfo PluginDirectory { get; }
        
        DirectoryInfo MappingsDirectory { get; }
    }
}