using Spectre.Console.Cli;
using System.ComponentModel;

namespace WSCT.EMV.CommandLine.Commands;

internal class CreateRsaKeySettings : CommandSettings
{
    [CommandOption(template: "--out", isRequired: false)]
    [Description("The output file name (default: emv-private-key.json)")]
    public required string OutputFileName { get; init; } = @"emv-private-key.json";

    [CommandOption(template: "--overwrite [BOOL]")]
    [Description("Whether to overwrite the output file if it already exists")]
    [DefaultValue(true)]
    public required FlagValue<bool> Overwrite { get; init; }

    [CommandOption(template: "--size", isRequired: true)]
    [Description("The key size")]
    public required int Size { get; init; }
}
