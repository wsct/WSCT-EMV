using Microsoft.AspNetCore.Components;
using WSCT.GUI.Plugins.EMVExplorer.Services;

namespace WSCT.GUI.Plugins.EMVExplorer.Components
{
    public partial class TlvTreeNode
    {
        [Parameter, EditorRequired]
        public EmvTreeNode Node { get; set; } = default!;

        private void Toggle()
        {
            Node.IsExpanded = !Node.IsExpanded;
        }
    }
}
