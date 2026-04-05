using Spectre.Console.Cli;

namespace WSCT.EMV.CommandLine.Services;

/// <summary>
/// Type resolver for Spectre.Console.Cli (https://spectreconsole.net/cli/tutorials/dependency-injection-in-cli-apps).
/// </summary>
internal sealed class TypeResolver(IServiceProvider provider) : ITypeResolver
{
    public object? Resolve(Type? type) => type == null ? null : provider.GetService(type);
}