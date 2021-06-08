using System.IO;

namespace Plugin.Constants.Paths.Abstractions
{
    public interface IPaths
    {
        DirectoryInfo PluginDirectory { get; }
        
        DirectoryInfo MappingsDirectory { get; }
    }
}