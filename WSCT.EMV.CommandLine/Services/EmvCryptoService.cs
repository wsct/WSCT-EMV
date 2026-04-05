using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using WSCT.EMV.CommandLine.Exceptions;
using WSCT.EMV.CommandLine.Model;
using WSCT.EMV.Personalization;
using WSCT.EMV.Security;
using WSCT.Helpers;
using WSCT.Helpers.Json;

namespace WSCT.EMV.CommandLine.Services;

public class EmvCryptoService(ILogger<EmvCryptoService> logger) : IEmvCryptoService
{
    public void CreateIccCertificate(string pathToIssuerPrivateKey, string pathToIccCertificateData, string outputFileName)
    {
        var issuerKey = pathToIssuerPrivateKey.CreateFromJsonFile<PrivateKey>();
        EMVException.ThrowIfNull(issuerKey, "Something went wrong when loading the CA key from file");

        var IccCertificateDataContainer = pathToIccCertificateData.CreateFromJsonFile<IccCertificateDataContainer>();
        EMVException.ThrowIfNull(IccCertificateDataContainer, "Something went wrong when loading the issuer certificate data from file");

        var iccPrivateKey = IccCertificateDataContainer.PathToIccKey.CreateFromJsonFile<PrivateKey>();
        EMVException.ThrowIfNull(iccPrivateKey, "Something went wrong when loading the issuer public key from file");

        var iccCertificateData = new IccCertificateData
        {
            IssuerIdentifier = IccCertificateDataContainer.IssuerIdentifier.ToHexa('\0'),
            PublicKeyAlgorithmIndicator = $"{IccCertificateDataContainer.IccPublicKeyAlgorithmIndicator:X2}",
            IccPrivateKey = iccPrivateKey,
            ExpirationDate = IccCertificateDataContainer.ExpirationDate.ToHexa('\0'),
            SerialNumber = IccCertificateDataContainer.SerialNumber.ToHexa('\0'),
            HashAlgorithmIndicator = $"{IccCertificateDataContainer.HashAlgorithmIndicator:X2}",
            ApplicationPan = IccCertificateDataContainer.PrimaryAccountNumber.ToHexa('\0')
        };

        var iccCertificateBuilder = new IccCertificateBuilder(iccCertificateData, issuerKey);

        iccCertificateBuilder.IccContext.WriteToJsonFile(outputFileName, indented: true);
    }

    /// <inheritdoc />
    public void CreateIssuerCertificate(string pathToCertificateAuthorityPrivateKey, string pathToIssuerCertificateData, string outputFileName)
    {
        var caKey = pathToCertificateAuthorityPrivateKey.CreateFromJsonFile<PrivateKey>();
        EMVException.ThrowIfNull(caKey, "Something went wrong when loading the CA key from file");

        var issuerCertificateDataContainer = pathToIssuerCertificateData.CreateFromJsonFile<IssuerCertificateDataContainer>();
        EMVException.ThrowIfNull(issuerCertificateDataContainer, "Something went wrong when loading the issuer certificate data from file");

        var issuerPrivateKey = issuerCertificateDataContainer.PathToIssuerKey.CreateFromJsonFile<PrivateKey>();
        EMVException.ThrowIfNull(issuerPrivateKey, "Something went wrong when loading the issuer public key from file");

        var issuerCertificateData = new IssuerCertificateData
        {
            IssuerIdentifier = issuerCertificateDataContainer.IssuerIdentifier.ToHexa('\0'),
            PublicKeyAlgorithmIndicator = $"{issuerCertificateDataContainer.IssuerPublicKeyAlgorithmIndicator:X2}",
            IssuerPrivateKey = issuerPrivateKey,
            ExpirationDate = issuerCertificateDataContainer.ExpirationDate.ToHexa('\0'),
            SerialNumber = issuerCertificateDataContainer.SerialNumber.ToHexa('\0'),
            HashAlgorithmIndicator = $"{issuerCertificateDataContainer.HashAlgorithmIndicator:X2}",
            CaPublicKeyIndex = $"{issuerCertificateDataContainer.CertificateAuthorityIndex:X2}"
        };

        var issuerCertificateBuilder = new IssuerCertificateBuilder(issuerCertificateData, caKey);

        issuerCertificateBuilder.IssuerContext.WriteToJsonFile(outputFileName, indented: true);
    }

    /// <inheritdoc />
    public void CreateRsaKey(int keySize, string outputFileName)
    {
        logger.LogTrace("Generating Key: Type: RSA, Size: {KeySize}, output: {keyFileName}", keySize, outputFileName);

        // Note: the exponent is always 0x010001 (65537) using this API
        using RSA rsa = RSA.Create(keySize);

        RSAParameters rsaPrivate = rsa.ExportParameters(includePrivateParameters: true);

        EMVException.ThrowIfNull(rsaPrivate.Modulus, "RSA Modulus must not be null");
        EMVException.ThrowIfNull(rsaPrivate.D, "RSA Private Exponent must not be null");
        EMVException.ThrowIfNull(rsaPrivate.Exponent, "RSA Public Exponent must not be null");

        var privateKey = new PrivateKey
        {
            Modulus = rsaPrivate.Modulus.ToHexa('\0'),
            PrivateExponent = rsaPrivate.D.ToHexa('\0'),
            PublicExponent = rsaPrivate.Exponent.ToHexa('\0')
        };

        privateKey.WriteToJsonFile(outputFileName, indented: true);
    }
}
