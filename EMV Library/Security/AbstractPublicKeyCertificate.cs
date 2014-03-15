using System;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV certificate for a Public Key
    /// </summary>
    public abstract class AbstractPublicKeyCertificate : AbstractSignatureContainer
    {
        #region >> Fields

        private Int32 _identifierLength;

        private Byte[] _certificateExpirationDate;
        private Byte[] _certificateSerialNumber;
        private Byte[] _publicKeyorLeftmostDigitsofthePublicKey;

        #endregion

        #region >> Properties

        /// <summary>
        /// Certificate Expiration Date (2): MMYY after which this certificate is invalid 
        /// </summary>
        public Byte[] certificateExpirationDate
        {
            get
            {
                if (_certificateExpirationDate == null)
                {
                    _certificateExpirationDate = new Byte[2];
                    Array.Copy(_recovered, 2 + _identifierLength, _certificateExpirationDate, 0, 2);
                }
                return _certificateExpirationDate;
            }
        }

        /// <summary>
        /// Certificate Serial Number (3): Binary number unique to this certificate assigned by the certification authority
        /// </summary>
        public Byte[] certificateSerialNumber
        {
            get
            {
                if (_certificateSerialNumber == null)
                {
                    _certificateSerialNumber = new Byte[3];
                    Array.Copy(_recovered, 2 + _identifierLength + 2, _certificateSerialNumber, 0, 3);
                }
                return _certificateSerialNumber;
            }
        }

        /// <summary>
        /// Public Key (1): Identifies the digital signature algorithm to be used with the Public KeyAlgorithm Indicator
        /// </summary>
        public Byte publicKeyAlgorithmIndicator
        {
            get { return _recovered[2 + _identifierLength + 6]; }
        }

        /// <summary>
        /// Public Key Length (1): Identifies the length of the Public Key Modulus in bytes 
        /// </summary>
        public Byte publicKeyLength
        {
            get { return _recovered[2 + _identifierLength + 7]; }
        }

        /// <summary>
        /// Public Key Exponent Length (1): Identifies the length of the Public Key Exponent in bytes
        /// </summary>
        public Byte publicKeyExponentLength
        {
            get { return _recovered[2 + _identifierLength + 8]; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Byte[] publicKeyorLeftmostDigitsofthePublicKey
        {
            get
            {
                if (_publicKeyorLeftmostDigitsofthePublicKey == null)
                {
                    if (publicKeyLength <= keyLength - (22 + 10 + _identifierLength))
                    {
                        _publicKeyorLeftmostDigitsofthePublicKey = new Byte[publicKeyLength];
                        Array.Copy(_recovered, 2 + _identifierLength + 9, _publicKeyorLeftmostDigitsofthePublicKey, 0, publicKeyLength);
                    }
                    else
                    {
                        _publicKeyorLeftmostDigitsofthePublicKey = new Byte[keyLength - (22 + 10 + _identifierLength)];
                        Array.Copy(_recovered, 2 + _identifierLength + 9, _publicKeyorLeftmostDigitsofthePublicKey, 0, keyLength - (22 + 10 + _identifierLength));
                    }
                }
                return _publicKeyorLeftmostDigitsofthePublicKey;
            }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AbstractPublicKeyCertificate(Int32 identifierLength)
            : base(7 + identifierLength)
        {
            _identifierLength = identifierLength;
        }

        #endregion
    }
}
