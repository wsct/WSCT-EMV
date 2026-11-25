using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using WSCT.GUI.Plugins;
using WSCT.GUI.Plugins.EMVExplorer.Services;

namespace WSCT.GUI.Plugins.EMVExplorer
{
    public partial class EmvExplorerGui : IDisposable
    {
        private string _activeTab = "emv";
        private string _authSubTab = "sda";

        [Inject]
        private EmvExplorerSessionService Session { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        protected override void OnInitialized()
        {
            Session.OnStateChanged += HandleStateChanged;
            SharedData.CardChannelChangedEvent += HandleCardChannelChanged;
        }

        private void HandleStateChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        private void HandleCardChannelChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        private void OnPinEntryUsedChanged(ChangeEventArgs e)
        {
            Session.PinEntryUsed = e.Value is bool b && b;
            if (!Session.PinEntryUsed)
            {
                Session.PinEntry = string.Empty;
            }
        }

        private async Task SaveCardLogAsync()
        {
            var content = Session.BuildCardLogExportText();
            await JSRuntime.InvokeVoidAsync("wsctHelpers.downloadFile", "emv-card-log.log", "text/plain", content);
        }

        private async Task SaveDetailedLogsAsync()
        {
            var content = Session.BuildDetailedLogsExportText();
            await JSRuntime.InvokeVoidAsync("wsctHelpers.downloadFile", "emv-detailed-logs.log", "text/plain", content);
        }

        private static bool IsValidHex(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            var compact = text.Replace(" ", string.Empty);
            if (compact.Length % 2 != 0)
            {
                return false;
            }

            foreach (var c in compact)
            {
                if (!Uri.IsHexDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            Session.OnStateChanged -= HandleStateChanged;
            SharedData.CardChannelChangedEvent -= HandleCardChannelChanged;
        }
    }
}
