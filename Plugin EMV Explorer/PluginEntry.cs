using WSCT.GUI.Plugins.EMVExplorer.Resources;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    /// <summary>
    /// Plugin Entry Class.
    /// </summary>
    [PluginEntry(Name = nameof(Lang.EMVPluginName), Description = nameof(Lang.EMVPluginDescription), ResourceType = typeof(Lang))]
    public class PluginEntry : GenericPluginEntry<Gui>
    {
    }
}