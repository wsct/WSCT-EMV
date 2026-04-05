using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Services;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateIccCertificateCommand(IEmvConsoleService emvConsoleService)
     : Command<CreateIccCertificateSettings>
{
    protected override int Execute(CommandContext context, CreateIccCertificateSettings settings, CancellationToken cancellationToken)
    {
        var overwrite = settings.Overwrite.IsSet && settings.Overwrite.Value;

        var result = emvConsoleService.CreateIccCertificate(settings.PathToIssuerPrivateKey, settings.PathToIccCertificateData, settings.OutputFileName, overwrite);

        return result ? 0 : 1;
    }
}