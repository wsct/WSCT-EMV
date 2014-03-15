using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents a container for EMV data signatures (public key certificate, data authentication)
    /// </summary>
    public abstract class AbstractSignatureContainer
    {
        #region >> Fields

        /// <summary>
        /// Size of the "Issuer Identifier" (Issuer) or PAN (ICC) field
        /// </summary>
        private Int32 _hashAlgorithmIndicatorOffset;

        /// <summary>
        /// Length of the Key
        /// </summary>
        protected Int32 keyLength;

        /// <summary>
        /// Recovered data from certificate
        /// </summary>
        protected Byte[] _recovered;

        private Byte[] _hashResult;

        #endregion

        #region >> Properties

        /// <summary>
        /// Accessor to raw recovered data from the EMV signature
        /// </summary>
        public Byte[] recovered
        {
            get { return _recovered; }
        }

        /// <summary>
        /// Recovered Data Header (1): Hex value '6A' 
        /// </summary>
        public Byte dataHeader
        {
            get { return _recovered[0]; }
        }

        /// <summary>
        /// Certificate Format (1)
        /// </summary>
        public Byte dataFormat
        {
            get { return _recovered[1]; }
        }

        /// <summary>
        /// Hash Algorithm Indicator (1): Identifies the hash algorithm used to produce the Hash Result in the digital signature scheme
        /// </summary>
        public Byte hashAlgorithmIndicator
        {
            get { return _recovered[_hashAlgorithmIndicatorOffset]; }
        }

        /// <summary>
        /// Hash Result: Hash of the Public Key and its related information
        /// </summary>
        public Byte[] hashResult
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
        /// Recovered Data Trailer: Hex value 'BC'
        /// </summary>
        public Byte dataTrailer
        {
            get { return _recovered[_recovered.Length - 1]; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractSignatureContainer(Int32 hashAlgorithmIndicatorOffset)
        {
            _hashAlgorithmIndicatorOffset = hashAlgorithmIndicatorOffset;
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// EMV recovery function: recovers data contained in the signature
        /// </summary>
        /// <param name="signature">Signature to recover data from</param>
        /// <param name="publicKey">Public key of the signing authority (see Bouncy Castle for details)</param>
        public void recoverFromSignature(Byte[] signature, RsaKeyParameters publicKey)
        {
            Cryptography cryptography = new Cryptography();
            keyLength = signature.Length;

            // recover data from the certificate
            _recovered = cryptography.RecoverMessage(signature, publicKey);

            // extract and check data recovered
            if (dataHeader != 0x6A)
                throw new EMVBadRecoveredDataException(String.Format("recoverFromCertificate: Recovered Data Header incorrect [{0:X2}]\n", dataHeader));

            if (dataTrailer != 0xBC)
                throw new EMVBadRecoveredDataException(String.Format("recoverFromCertificate: Recovered Data Trailer incorrect [{0:X2}]\n", dataTrailer));
        }

        /// <summary>
        /// EMV recovery function: recovers data contained in the certificate
        /// </summary>
        /// <param name="signature">Certificate to recover data from</param>
        /// <param name="modulus">Modulus of the public key of the signing authority</param>
        /// <param name="exponent">Exponent of the public key of the signing authority</param>
        public void recoverFromSignature(String signature, String modulus, String exponent)
        {
            // get the signing public key
            RsaKeyParameters publicKey = new RsaKeyParameters(false, new BigInteger(modulus, 16), new BigInteger(exponent, 16));

            recoverFromSignature(signature.fromHexa(), publicKey);
        }

        /// <summary>
        /// EMV recovery funcntion: recovers data contained in the certificate
        /// </summary>
        /// <param name="signature">Certificate to recover data from</param>
        /// <param name="publicKey">Public key of the signing authority</param>
        public void recoverFromSignature(Byte[] signature, PublicKey publicKey)
        {
            // get the signing public key
            RsaKeyParameters rsaPublicKey = new RsaKeyParameters(false, new BigInteger(publicKey.modulus, 16), new BigInteger(publicKey.exponent, 16));

            recoverFromSignature(signature, rsaPublicKey);
        }

        /// <summary>
        /// Compute hash value of the certificate
        /// </summary>
        /// <param name="data">List of Byte[] to be concatenated sequencially and hashed</param>
        /// <returns>Hash value</returns>
        public Byte[] computeHash(List<Byte[]> data)
        {
            HashAlgorithmFactory hashFactory = new HashAlgorithmFactory();
            IHashAlgorithmProvider hashProvider = hashFactory.getProvider(hashAlgorithmIndicator);
            return hashProvider.computeHash(data);
        }

        #endregion
    }
}
