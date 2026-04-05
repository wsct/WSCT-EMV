using Spectre.Console.Cli;
using System.ComponentModel;
using WSCT.EMV.CommandLine.Converters;

namespace WSCT.EMV.CommandLine.Commands;

internal class SetIssuerCertificateDataSettings : CommandSettings
{
    [CommandOption(template: "--out", isRequired: false)]
    [Description("The output file name (default: issuer-certificate-data.json)")]
    public required string OutputFileName { get; init; } = @"issuer-certificate-data.json";

    [CommandOption(template: "--overwrite [BOOL]")]
    [Description("Whether to overwrite the output file if it already exists")]
    [DefaultValue(true)]
    public required FlagValue<bool> Overwrite { get; init; }

    [CommandOption(template: "--ca-index", isRequired: true)]
    [Description("The Certificate Authority Index")]
    [TypeConverter(typeof(HexaStringToByteConverter))]
    public required byte CertificateAuthorityIndex { get; init; }

    [CommandOption(template: "--hash-algo", isRequired: true)]
    [Description("The Hash Algorithm Indicator ('01' for SHA-1)")]
    [TypeConverter(typeof(HexaStringToByteConverter))]
    public required byte HashAlgorithmIndicator { get; init; }

    [CommandOption(template: "--id", isRequired: true)]
    [Description("The Issuer Identifier (Leftmost 3-8 digits from the PAN, padded to the right with 'F's)")]
    [TypeConverter(typeof(HexaStringToByteArrayConverter))]
    public required byte[] IssuerIdentifier { get; init; }

    [CommandOption(template: "--expire", isRequired: true)]
    [Description("The Certificate Expiration Date (MMYY after which this certificate is invalid)")]
    [TypeConverter(typeof(HexaStringToByteArrayConverter))]
    public required byte[] ExpirationDate { get; init; }

    [CommandOption(template: "--serial", isRequired: true)]
    [Description("The Certificate Serial Number (3 bytes)")]
    [TypeConverter(typeof(HexaStringToByteArrayConverter))]
    public required byte[] SerialNumber { get; init; }

    [CommandOption(template: "--key-algo", isRequired: true)]
    [Description("The Issuer Public Key Algorithm Indicator ('01' for RSA)")]
    [TypeConverter(typeof(HexaStringToByteConverter))]
    public required byte IssuerPublicKeyAlgorithmIndicator { get; init; }

    [CommandOption(template: "--key", isRequired: true)]
    [Description("Path to the Issuer Key file")]
    public string PathToIssuerKey { get; init; } = @"issuer-key.json";
}