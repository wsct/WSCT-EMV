namespace WSCT.EMV.CommandLine.Model;

internal class IssuerCertificateDataContainer
{
    public required byte CertificateAuthorityIndex { get; init; }

    public required byte HashAlgorithmIndicator { get; init; }

    public required byte[] IssuerIdentifier { get; init; }

    public required byte[] ExpirationDate { get; init; }

    public required byte[] SerialNumber { get; init; }

    public required byte IssuerPublicKeyAlgorithmIndicator { get; init; }

    public required string PathToIssuerKey { get; init; }
}
