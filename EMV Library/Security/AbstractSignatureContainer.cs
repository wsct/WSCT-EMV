using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a container for EMV data signatures (public key certificate, data authentication).
    /// </summary>
    public abstract class AbstractSignatureContainer
    {
        #region >> Fields

        private readonly Int32 _hashAlgorithmIndicatorOffset;

        /// <summary>
        /// Length of the Key.
        /// </summary>
        protected Int32 KeyLength;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to raw recovered data from the EMV signature.
        /// </summary>
        public byte[] Recovered { get; private set; }

        /// <summary>
        /// Recovered Data Header (1): Hex value '6A'.
        /// </summary>
        public byte DataHeader { get; private set; }

        /// <summary>
        /// Certificate Format (1).
        /// </summary>
        public byte DataFormat { get; protected set; }

        /// <summary>
        /// Hash Algorithm Indicator (1): Identifies the hash algorithm used to produce the Hash Result in the digital signature scheme.
        /// </summary>
        public byte HashAlgorithmIndicator { get; set; }

        /// <summary>
        /// Hash Result: Hash of the Public Key and its related information.
        /// </summary>
        public byte[] HashResult { get; private set; }

        /// <summary>
        /// Recovered Data Trailer: Hex value 'BC'.
        /// </summary>
        public byte DataTrailer { get; private set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected AbstractSignatureContainer(Int32 hashAlgorithmIndicatorOffset)
        {
            _hashAlgorithmIndicatorOffset = hashAlgorithmIndicatorOffset;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// EMV recovery funcntion: recovers data contained in the certificate.
        /// </summary>
        /// <param name="signature">Certificate to recover data from.</param>
        /// <param name="publicKey">Public key of the signing authority.</param>
        public void RecoverFromSignature(byte[] signature, PublicKey publicKey)
        {
            var rsaPublicKey = new RsaKeyParameters(false, new BigInteger(publicKey.Modulus, 16), new BigInteger(publicKey.Exponent, 16));

            RecoverFromSignature(signature, rsaPublicKey);
        }

        /// <summary>
        /// Compute hash value of the certificate.
        /// </summary>
        /// <param name="data">List of byte[] to be concatenated sequencially and hashed.</param>
        /// <returns>Hash value.</returns>
        public byte[] ComputeHash(List<byte[]> data)
        {
            var hashFactory = new HashAlgorithmFactory();
            var hashProvider = hashFactory.GetProvider(HashAlgorithmIndicator);

            return hashProvider.ComputeHash(data);
        }

        /// <summary>
        /// Compute the signed certificate.
        /// </summary>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        public byte[] GenerateCertificate(PublicKey privateKey)
        {
            var rsaPrivateKey = new RsaKeyParameters(false, new BigInteger(privateKey.Modulus, 16), new BigInteger(privateKey.Exponent, 16));

            return GetDataToSign(rsaPrivateKey.Modulus.BitLength / 8).GenerateSignature(rsaPrivateKey);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="privateKeyLength"></param>
        /// <returns></returns>
        protected abstract byte[] GetDataToSign(int privateKeyLength);

        /// <summary>
        /// TODO
        /// </summary>
        protected abstract void OnRecoverFromSignature();

        #endregion

        /// <summary>
        /// EMV recovery function: recovers data contained in the signature.
        /// </summary>
        /// <param name="signature">Signature to recover data from.</param>
        /// <param name="publicKey">Public key of the signing authority (see Bouncy Castle for details).</param>
        private void RecoverFromSignature(byte[] signature, RsaKeyParameters publicKey)
        {
            KeyLength = signature.Length;

            // recovered data from the certificate
            Recovered = signature.RecoverMessage(publicKey);

            // extract and check data recovered
            DataHeader = Recovered[0];
            if (DataHeader != 0x6A)
            {
                throw new EMVBadRecoveredDataException(String.Format("RecoverFromCertificate: Recovered Data Header incorrect [{0:X2}]\n", DataHeader));
            }

            DataTrailer = Recovered[Recovered.Length - 1];
            if (DataTrailer != 0xBC)
            {
                throw new EMVBadRecoveredDataException(String.Format("RecoverFromCertificate: Recovered Data Trailer incorrect [{0:X2}]\n", DataTrailer));
            }

            HashAlgorithmIndicator = Recovered[_hashAlgorithmIndicatorOffset];

            DataFormat = Recovered[1];

            HashResult = new byte[20];
            Array.Copy(Recovered, Recovered.Length - 21, HashResult, 0, 20);

            OnRecoverFromSignature();
        }
    }
}