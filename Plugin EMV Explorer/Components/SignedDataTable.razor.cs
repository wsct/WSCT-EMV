using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using WSCT.GUI.Plugins.EMVExplorer.Services;

namespace WSCT.GUI.Plugins.EMVExplorer.Components
{
    public partial class SignedDataTable
    {
        [Parameter, EditorRequired]
        public IReadOnlyList<EmvSignedDataItem> Items { get; set; } = [];
    }
}
