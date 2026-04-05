namespace WSCT.EMV.CommandLine.Model;

internal class IccCertificateDataContainer
{
    public required byte[] PrimaryAccountNumber { get; init; }

    public required byte HashAlgorithmIndicator { get; init; }

    public required byte[] IssuerIdentifier { get; init; }

    public required byte[] ExpirationDate { get; init; }

    public required byte[] SerialNumber { get; init; }

    public required byte IccPublicKeyAlgorithmIndicator { get; init; }

    public required string PathToIccKey { get; init; }
}
