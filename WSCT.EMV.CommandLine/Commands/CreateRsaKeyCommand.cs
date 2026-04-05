using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Services;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateRsaKeyCommand(IEmvConsoleService emvConsoleService)
    : Command<CreateRsaKeySettings>
{
    protected override int Execute(CommandContext context, CreateRsaKeySettings settings, CancellationToken cancellationToken)
    {
        var overwrite = settings.Overwrite.IsSet && settings.Overwrite.Value;

        var result = emvConsoleService.CreateRsaKey(settings.Size, settings.OutputFileName, overwrite);

        return result ? 0 : 1;
    }
}
