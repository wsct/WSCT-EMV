using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace WSCT.EMV.CommandLine.Services;

/// <summary>
/// Type registrar for Spectre.Console.Cli (https://spectreconsole.net/cli/tutorials/dependency-injection-in-cli-apps).
/// </summary>
internal sealed class TypeRegistrar(IServiceCollection services) : ITypeRegistrar
{
    public ITypeResolver Build() => new TypeResolver(services.BuildServiceProvider());

    public void Register(Type service, Type implementation) => services.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => services.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> factory) => services.AddSingleton(service, _ => factory());
}
