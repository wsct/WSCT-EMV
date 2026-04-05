using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Model;
using WSCT.EMV.CommandLine.Services;

namespace WSCT.EMV.CommandLine.Commands;

internal class SetIccCertificateDataCommand(IEmvConsoleService emvConsoleService)
    : Command<SetIccCertificateDataSettings>
{
    protected override int Execute(CommandContext context, SetIccCertificateDataSettings settings, CancellationToken cancellationToken)
    {
        var iccCertificateData = new IccCertificateDataContainer()
        {
            PrimaryAccountNumber = settings.PrimaryAccountNumber,
            HashAlgorithmIndicator = settings.HashAlgorithmIndicator,
            IssuerIdentifier = settings.IssuerIdentifier,
            ExpirationDate = settings.ExpirationDate,
            SerialNumber = settings.SerialNumber,
            IccPublicKeyAlgorithmIndicator = settings.IccPublicKeyAlgorithmIndicator,
            PathToIccKey = settings.PathToIccKey
        };

        var overwrite = settings.Overwrite.IsSet && settings.Overwrite.Value;

        var result = emvConsoleService.SetIccCertificateData(
            iccCertificateData,
            settings.OutputFileName,
            overwrite
        );

        return result ? 0 : 1;
    }
}