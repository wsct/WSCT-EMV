using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Model;
using WSCT.EMV.CommandLine.Services;

namespace WSCT.EMV.CommandLine.Commands;

internal class SetIssuerCertificateDataCommand(IEmvConsoleService emvConsoleService)
    : Command<SetIssuerCertificateDataSettings>
{
    protected override int Execute(CommandContext context, SetIssuerCertificateDataSettings settings, CancellationToken cancellationToken)
    {
        var issuerCertificateData = new IssuerCertificateDataContainer()
        {
            CertificateAuthorityIndex = settings.CertificateAuthorityIndex,
            HashAlgorithmIndicator = settings.HashAlgorithmIndicator,
            IssuerIdentifier = settings.IssuerIdentifier,
            ExpirationDate = settings.ExpirationDate,
            SerialNumber = settings.SerialNumber,
            IssuerPublicKeyAlgorithmIndicator = settings.IssuerPublicKeyAlgorithmIndicator,
            PathToIssuerKey = settings.PathToIssuerKey
        };

        var overwrite = settings.Overwrite.IsSet && settings.Overwrite.Value;

        var result = emvConsoleService.SetIssuerCertificateData(
            issuerCertificateData,
            settings.OutputFileName,
            overwrite
        );

        return result ? 0 : 1;
    }
}