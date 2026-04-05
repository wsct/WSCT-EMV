using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using WSCT.EMV.CommandLine.Commands;
using WSCT.EMV.CommandLine.Services;

var services = new ServiceCollection();

// Services
services.AddSingleton<IEmvCryptoService, EmvCryptoService>();
services.AddSingleton<IEmvConsoleService, EmvConsoleService>();

// Logging
services.AddLogging(configure => configure.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.IncludeScopes = true;
}));

// Spectre.Console.Cli
var registrar = new TypeRegistrar(services);

var app = new CommandApp(registrar);
app.Configure(config =>
{
    config.SetApplicationName("wsct-emv");

    config.AddBranch("create-key", createKey =>
    {
        createKey.AddCommand<CreateRsaKeyCommand>("rsa")
            .WithDescription("Create a new RSA key");
    });

    config.AddBranch("issuer", issuer =>
    {
        issuer.AddCommand<SetIssuerCertificateDataCommand>("set-data")
            .WithDescription("Set data for the issuer");
        issuer.AddCommand<CreateIssuerCertificateCommand>("create-cert")
            .WithDescription("Create the Issuer Public Key Certificate");
    });

    config.AddBranch("icc", icc =>
    {
        icc.AddCommand<SetIccCertificateDataCommand>("set-data")
            .WithDescription("Set data for the issuer");
        icc.AddCommand<CreateIccCertificateCommand>("create-cert")
            .WithDescription("Create the ICC Public Key Certificate");
    });
});

return await app.RunAsync(args);
