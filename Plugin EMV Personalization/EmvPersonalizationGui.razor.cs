using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using WSCT.GUI.Plugins;
using WSCT.GUI.Plugins.EMV.Personalization.Services;

namespace WSCT.GUI.Plugins.EMV.Personalization
{
    public partial class EmvPersonalizationGui : IDisposable
    {
        private const long MaxJsonUploadBytes = 5 * 1024 * 1024;

        [Inject]
        private EmvPersonalizationSessionService Session { get; set; } = default!;

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

        private async Task UploadEmvFilesAsync(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(10))
            {
                using var reader = new StreamReader(file.OpenReadStream(MaxJsonUploadBytes));
                var content = await reader.ReadToEndAsync();
                Session.SetUploadedEmvFile(file.Name, content);
            }
        }

        private async Task UploadPseFilesAsync(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles(10))
            {
                using var reader = new StreamReader(file.OpenReadStream(MaxJsonUploadBytes));
                var content = await reader.ReadToEndAsync();
                Session.SetUploadedPseFile(file.Name, content);
            }
        }

        private async Task DownloadLogsAsync()
        {
            var content = Session.BuildLogsExportText();
            await JSRuntime.InvokeVoidAsync("wsctHelpers.downloadFile", "emv-personalization.log", "text/plain", content);
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
