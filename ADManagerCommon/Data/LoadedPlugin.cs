using ADManager.Plugins;
using System.Reflection;

namespace ADManager.Common.Data
{
    public class LoadedPlugin
    {
        public LoadedPlugin(Assembly assembly, IPluginBase pluginInstance)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly), "Assembly cannot be null.");
            PluginBase = pluginInstance ?? throw new ArgumentNullException(nameof(pluginInstance), "Plugin instance cannot be null.");
        }

        public Assembly Assembly { get; }
        public IPluginBase PluginBase { get; }

    }
}