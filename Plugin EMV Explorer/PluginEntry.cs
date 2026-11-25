using Microsoft.Extensions.DependencyInjection;
using WSCT.GUI.Plugins.EMVExplorer.Resources;
using WSCT.GUI.Plugins.EMVExplorer.Services;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    /// <summary>
    /// Plugin Entry Class.
    /// </summary>
    [PluginEntry(Name = nameof(Lang.EMVPluginName), Description = nameof(Lang.EMVPluginDescription), ResourceType = typeof(Lang))]
    public class PluginEntry : GenericPluginEntry<EmvExplorerGui>
    {
        public override string Icon => "bi bi-credit-card-2-front";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EmvExplorerSessionService>();
        }
    }
}
