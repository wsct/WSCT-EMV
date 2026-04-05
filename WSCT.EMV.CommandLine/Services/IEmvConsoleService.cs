using WSCT.EMV.CommandLine.Model;

namespace WSCT.EMV.CommandLine.Services;

internal interface IEmvConsoleService
{
    bool CreateIccCertificate(string pathToIssuerPrivateKey, string pathToIccCertificateData, string outputFileName, bool overwrite);

    bool CreateIssuerCertificate(string pathToCertificateAuthorityPrivateKey, string pathToIssuerCertificateData, string outputFileName, bool overwrite);

    bool CreateRsaKey(int keySize, string outputFileName, bool overwrite);

    bool SetIccCertificateData(IccCertificateDataContainer iccCertificateData, string outputFileName, bool overwrite);

    bool SetIssuerCertificateData(IssuerCertificateDataContainer data, string outputFileName, bool overwrite);
}
