using Microsoft.AspNetCore.Components;

namespace WSCT.GUI.Plugins.EMVExplorer.Components
{
    public partial class TvrBitRow
    {
        [Parameter, EditorRequired]
        public string Label { get; set; } = string.Empty;

        [Parameter]
        public bool IsSet { get; set; }

        [Parameter]
        public bool IsRfu { get; set; }
    }
}
