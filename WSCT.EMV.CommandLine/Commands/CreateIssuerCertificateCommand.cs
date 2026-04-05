using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Services;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateIssuerCertificateCommand(IEmvConsoleService emvConsoleService)
     : Command<CreateIssuerCertificateSettings>
{
    protected override int Execute(CommandContext context, CreateIssuerCertificateSettings settings, CancellationToken cancellationToken)
    {
        var overwrite = settings.Overwrite.IsSet && settings.Overwrite.Value;

        var result = emvConsoleService.CreateIssuerCertificate(settings.PathToCertificateAuthorityPrivateKey, settings.PathToIssuerCertificateData, settings.OutputFileName, overwrite);

        return result ? 0 : 1;
    }
}