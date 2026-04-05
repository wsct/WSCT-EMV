using Spectre.Console.Cli;
using System.ComponentModel;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateIssuerCertificateSettings : CommandSettings
{
    [CommandOption(template: "--out", isRequired: false)]
    [Description("The output file name (default: emv-issuer-context.json)")]
    public required string OutputFileName { get; init; } = @"emv-issuer-context.json";

    [CommandOption(template: "--overwrite [BOOL]")]
    [Description("Whether to overwrite the output file if it already exists")]
    [DefaultValue(true)]
    public required FlagValue<bool> Overwrite { get; init; }

    [CommandOption(template: "--key-ca", isRequired: false)]
    [Description("Path to the Certificate Authority's private key file (default: ca-private-key.json)")]
    public required string PathToCertificateAuthorityPrivateKey { get; init; } = @"ca-private-key.json";

    [CommandOption(template: "--data", isRequired: false)]
    [Description("Path to the Issuer Public Key file (default: issuer-certificate-data.json")]
    public required string PathToIssuerCertificateData { get; init; } = @"issuer-certificate-data.json";
}