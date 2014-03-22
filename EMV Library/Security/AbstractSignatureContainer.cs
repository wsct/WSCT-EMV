using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using WSCT.Helpers;

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

        /// <summary>
        /// Recovered data from certificate.
        /// </summary>
        protected Byte[] _recovered;

        private Byte[] _hashResult;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to raw recovered data from the EMV signature.
        /// </summary>
        public Byte[] Recovered
        {
            get { return _recovered; }
        }

        /// <summary>
        /// Recovered Data Header (1): Hex value '6A'.
        /// </summary>
        public Byte DataHeader
        {
            get { return _recovered[0]; }
        }

        /// <summary>
        /// Certificate Format (1).
        /// </summary>
        public Byte DataFormat
        {
            get { return _recovered[1]; }
        }

        /// <summary>
        /// Hash Algorithm Indicator (1): Identifies the hash algorithm used to produce the Hash Result in the digital signature scheme.
        /// </summary>
        public Byte HashAlgorithmIndicator
        {
            get { return _recovered[_hashAlgorithmIndicatorOffset]; }
        }

        /// <summary>
        /// Hash Result: Hash of the Public Key and its related information.
        /// </summary>
        public Byte[] HashResult
        {
            get
            {
                if (_hashResult == null)
                {
                    _hashResult = new Byte[20];
                    Array.Copy(_recovered, _recovered.Length - 21, _hashResult, 0, 20);
                }
                return _hashResult;
            }
        }

        /// <summary>
        /// Recovered Data Trailer: Hex value 'BC'.
        /// </summary>
        public Byte DataTrailer
        {
            get { return _recovered[_recovered.Length - 1]; }
        }

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
        /// EMV recovery function: recovers data contained in the signature.
        /// </summary>
        /// <param name="signature">Signature to recover data from.</param>
        /// <param name="publicKey">Public key of the signing authority (see Bouncy Castle for details).</param>
        public void RecoverFromSignature(Byte[] signature, RsaKeyParameters publicKey)
        {
            var cryptography = new Cryptography();
            KeyLength = signature.Length;

            // recover data from the certificate
            _recovered = cryptography.RecoverMessage(signature, publicKey);

            // extract and check data recovered
            if (DataHeader != 0x6A)
                throw new EMVBadRecoveredDataException(String.Format("recoverFromCertificate: Recovered Data Header incorrect [{0:X2}]\n", DataHeader));

            if (DataTrailer != 0xBC)
                throw new EMVBadRecoveredDataException(String.Format("recoverFromCertificate: Recovered Data Trailer incorrect [{0:X2}]\n", DataTrailer));
        }

        /// <summary>
        /// EMV recovery function: recovers data contained in the certificate.
        /// </summary>
        /// <param name="signature">Certificate to recover data from.</param>
        /// <param name="modulus">Modulus of the public key of the signing authority.</param>
        /// <param name="exponent">Exponent of the public key of the signing authority.</param>
        public void RecoverFromSignature(String signature, String modulus, String exponent)
        {
            // get the signing public key
            var publicKey = new RsaKeyParameters(false, new BigInteger(modulus, 16), new BigInteger(exponent, 16));

            RecoverFromSignature(signature.fromHexa(), publicKey);
        }

        /// <summary>
        /// EMV recovery funcntion: recovers data contained in the certificate.
        /// </summary>
        /// <param name="signature">Certificate to recover data from.</param>
        /// <param name="publicKey">Public key of the signing authority.</param>
        public void RecoverFromSignature(Byte[] signature, PublicKey publicKey)
        {
            // get the signing public key
            var rsaPublicKey = new RsaKeyParameters(false, new BigInteger(publicKey.Modulus, 16), new BigInteger(publicKey.Exponent, 16));

            RecoverFromSignature(signature, rsaPublicKey);
        }

        /// <summary>
        /// Compute hash value of the certificate.
        /// </summary>
        /// <param name="data">List of Byte[] to be concatenated sequencially and hashed.</param>
        /// <returns>Hash value.</returns>
        public Byte[] ComputeHash(List<Byte[]> data)
        {
            var hashFactory = new HashAlgorithmFactory();
            var hashProvider = hashFactory.GetProvider(HashAlgorithmIndicator);
            return hashProvider.ComputeHash(data);
        }

        #endregion
    }
}
