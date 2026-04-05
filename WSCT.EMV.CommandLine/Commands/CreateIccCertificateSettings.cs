using Spectre.Console.Cli;
using System.ComponentModel;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateIccCertificateSettings : CommandSettings
{
    [CommandOption(template: "--out", isRequired: false)]
    [Description("The output file name (default: emv-icc-context.json)")]
    public required string OutputFileName { get; init; } = @"emv-icc-context.json";

    [CommandOption(template: "--overwrite [BOOL]")]
    [Description("Whether to overwrite the output file if it already exists")]
    [DefaultValue(true)]
    public required FlagValue<bool> Overwrite { get; init; }

    [CommandOption(template: "--key-issuer", isRequired: false)]
    [Description("Path to the Issuer's private key file (default: issuer-private-key.json)")]
    public required string PathToIssuerPrivateKey { get; init; } = @"issuer-private-key.json";

    [CommandOption(template: "--data", isRequired: false)]
    [Description("Path to the ICC Public Key file (default: icc-certificate-data.json")]
    public required string PathToIccCertificateData { get; init; } = @"icc-certificate-data.json";
}