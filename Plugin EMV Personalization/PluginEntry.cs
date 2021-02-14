using WSCT.GUI.Plugins.EMV.Personalization.Resources;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    /// <inheritdoc />
    /// <summary>
    /// Plugin Entry point dedicated to create and show associated GUI.
    /// </summary>
    [PluginEntry(Name = nameof(Lang.ControllerPluginName), Description = nameof(Lang.ControllerPluginDescription), ResourceType = typeof(Lang))]
    public class PluginEntry : GenericPluginEntry<Gui>
    {
    }
}