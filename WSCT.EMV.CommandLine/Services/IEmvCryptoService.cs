namespace WSCT.EMV.CommandLine.Services;

public interface IEmvCryptoService
{
    void CreateIccCertificate(string pathToIssuerPrivateKey, string pathToIccCertificateData, string outputFileName);

    void CreateIssuerCertificate(string pathToCertificateAuthorityPrivateKey, string pathToIssuerCertificateData, string outputFileName);

    void CreateRsaKey(int keySize, string outputFileName);
}