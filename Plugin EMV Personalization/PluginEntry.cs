using Microsoft.Extensions.DependencyInjection;
using WSCT.GUI.Plugins.EMV.Personalization.Resources;
using WSCT.GUI.Plugins.EMV.Personalization.Services;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    /// <inheritdoc />
    /// <summary>
    /// Plugin Entry point dedicated to create and show associated GUI.
    /// </summary>
    [PluginEntry(Name = nameof(Lang.ControllerPluginName), Description = nameof(Lang.ControllerPluginDescription), ResourceType = typeof(Lang))]
    public class PluginEntry : GenericPluginEntry<EmvPersonalizationGui>
    {
        public override string Icon => "bi bi-sliders";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<EmvPersonalizationSessionService>();
        }
    }
}
