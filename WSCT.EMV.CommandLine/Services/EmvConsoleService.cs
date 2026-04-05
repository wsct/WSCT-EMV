using Spectre.Console;
using WSCT.EMV.CommandLine.Model;
using WSCT.Helpers.Json;

namespace WSCT.EMV.CommandLine.Services;

internal class EmvConsoleService(IEmvCryptoService cryptoService) : IEmvConsoleService
{
    public bool CreateIccCertificate(string pathToIssuerPrivateKey, string pathToIccCertificateData, string outputFileName, bool overwrite)
    {
        AnsiConsole.MarkupLine($"[yellow]Generating ICC certificate[/]");

        if (!CheckOutputFile(outputFileName, overwrite))
        {
            return false;
        }

        if (!File.Exists(pathToIssuerPrivateKey))
        {
            AnsiConsole.MarkupLine($"[red]Issuer Key file not found: '{pathToIssuerPrivateKey}'.[/]");
            return false;
        }

        if (!File.Exists(pathToIccCertificateData))
        {
            AnsiConsole.MarkupLine($"[red]Icc Key file not found: '{pathToIccCertificateData}'.[/]");
            return false;
        }

        try
        {
            cryptoService.CreateIccCertificate(pathToIssuerPrivateKey, pathToIccCertificateData, outputFileName);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine($"[red]{exception.GetType().Name}: {exception.Message}[/]");
            return false;
        }

        AnsiConsole.MarkupLine($"[yellow]Issuer certificate successfully written to '{outputFileName}'.[/]");

        return true;
    }

    public bool CreateIssuerCertificate(string pathToCertificateAuthorityPrivateKey, string pathToIssuerCertificateData, string outputFileName, bool overwrite)
    {
        AnsiConsole.MarkupLine($"[yellow]Generating Issuer certificate[/]");

        if (!CheckOutputFile(outputFileName, overwrite))
        {
            return false;
        }

        if (!File.Exists(pathToCertificateAuthorityPrivateKey))
        {
            AnsiConsole.MarkupLine($"[red]CA Key file not found: '{pathToCertificateAuthorityPrivateKey}'.[/]");
            return false;
        }

        if (!File.Exists(pathToIssuerCertificateData))
        {
            AnsiConsole.MarkupLine($"[red]Issuer Key file not found: '{pathToIssuerCertificateData}'.[/]");
            return false;
        }

        try
        {
            cryptoService.CreateIssuerCertificate(pathToCertificateAuthorityPrivateKey, pathToIssuerCertificateData, outputFileName);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine($"[red]{exception.GetType().Name}: {exception.Message}[/]");
            return false;
        }

        AnsiConsole.MarkupLine($"[yellow]Issuer certificate successfully written to '{outputFileName}'.[/]");

        return true;
    }

    public bool CreateRsaKey(int keySize, string outputFileName, bool overwrite)
    {
        AnsiConsole.MarkupLine($"[yellow]Generating RSA key[/]");

        if (!CheckOutputFile(outputFileName, overwrite))
        {
            return false;
        }

        try
        {
            cryptoService.CreateRsaKey(keySize, outputFileName);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine($"[red]{exception.GetType().Name}: {exception.Message}[/]");
            return false;
        }

        AnsiConsole.MarkupLine($"[yellow]RSA key successfully written to '{outputFileName}'.[/]");

        return true;
    }

    public bool SetIccCertificateData(IccCertificateDataContainer data, string outputFileName, bool overwrite)
    {
        AnsiConsole.MarkupLine($"[yellow]Building data for ICC Public Key Certificate generation[/]");

        if (!CheckOutputFile(outputFileName, overwrite))
        {
            return false;
        }

        if (data.HashAlgorithmIndicator != 0x01)
        {
            AnsiConsole.MarkupLine("[red]Currently unsupported Hash Algorithm Indicator.[/]");
            return false;
        }

        if (data.IccPublicKeyAlgorithmIndicator != 0x01)
        {
            AnsiConsole.MarkupLine("[red]Currently unsupported Icc Public Key Algorithm Indicator.[/]");
            return false;
        }

        if (data.IssuerIdentifier.Length != 0x04)
        {
            AnsiConsole.MarkupLine("[red]Issuer Identifier length unsupported (must be 4 bytes long).[/]");
            return false;
        }

        if (data.ExpirationDate.Length != 0x02)
        {
            AnsiConsole.MarkupLine("[red]Invalid Expiration Date length (must be 2 bytes long).[/]");
            return false;
        }

        if (data.SerialNumber.Length != 3)
        {
            AnsiConsole.MarkupLine("[red]Invalid Serial Number (must be 3 bytes long).[/]");
            return false;
        }

        if (!File.Exists(data.PathToIccKey))
        {
            AnsiConsole.MarkupLine($"[red]The file '{data.PathToIccKey}' does not exist.[/]");
            return false;
        }

        data.WriteToJsonFile(outputFileName);

        AnsiConsole.MarkupLine($"[yellow]Issuer certificate data successfully written to '{outputFileName}'.[/]");

        return true;

    }

    public bool SetIssuerCertificateData(IssuerCertificateDataContainer data, string outputFileName, bool overwrite)
    {
        AnsiConsole.MarkupLine($"[yellow]Building data for Issuer Public Key Certificate generation[/]");

        if (!CheckOutputFile(outputFileName, overwrite))
        {
            return false;
        }

        if (data.HashAlgorithmIndicator != 0x01)
        {
            AnsiConsole.MarkupLine("[red]Currently unsupported Hash Algorithm Indicator.[/]");
            return false;
        }

        if (data.IssuerPublicKeyAlgorithmIndicator != 0x01)
        {
            AnsiConsole.MarkupLine("[red]Currently unsupported Issuer Public Key Algorithm Indicator.[/]");
            return false;
        }

        if (data.IssuerIdentifier.Length != 0x04)
        {
            AnsiConsole.MarkupLine("[red]Issuer Identifier length unsupported (must be 4 bytes long).[/]");
            return false;
        }

        if (data.ExpirationDate.Length != 0x02)
        {
            AnsiConsole.MarkupLine("[red]Invalid Expiration Date length (must be 2 bytes long).[/]");
            return false;
        }

        if (data.SerialNumber.Length != 3)
        {
            AnsiConsole.MarkupLine("[red]Invalid Serial Number (must be 3 bytes long).[/]");
            return false;
        }

        if (!File.Exists(data.PathToIssuerKey))
        {
            AnsiConsole.MarkupLine($"[red]The file '{data.PathToIssuerKey}' does not exist.[/]");
            return false;
        }

        data.WriteToJsonFile(outputFileName);

        AnsiConsole.MarkupLine($"[yellow]Issuer certificate data successfully written to '{outputFileName}'.[/]");

        return true;
    }

    private static bool CheckOutputFile(string outputFileName, bool overwrite)
    {
        if (!overwrite && File.Exists(outputFileName))
        {
            AnsiConsole.MarkupLine($"[red]The file '{outputFileName}' already exist:[/] Use --overwrite to force the execution.");

            return false;
        }

        return true;
    }
}