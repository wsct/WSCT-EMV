using System;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Interchange Profile of an EMV application.
    /// </summary>
    public class ApplicationInterchangeProfile : AbstractTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Informs if CDA is supported.
        /// </summary>
        public Boolean Cda
        {
            get { return (tlv.value[0] & 0x01) != 0x00; }
        }

        /// <summary>
        /// Informs if DDA is supported.
        /// </summary>
        public Boolean Dda
        {
            get { return (tlv.value[0] & 0x20) != 0x00; }
        }

        /// <summary>
        /// Informs if SDA is supported.
        /// </summary>
        public Boolean Sda
        {
            get { return (tlv.value[0] & 0x40) != 0x00; }
        }

        /// <summary>
        /// Informs if Cardholder verification is supported.
        /// </summary>
        public Boolean CardholderVerification
        {
            get { return (tlv.value[0] & 0x10) != 0x00; }
        }

        /// <summary>
        /// Informs if Issuer authentication is supported.
        /// </summary>
        public Boolean IssuerAuthentication
        {
            get { return (tlv.value[0] & 0x04) != 0x00; }
        }

        /// <summary>
        /// Informs if Terminal Risk Management is to be performed.
        /// </summary>
        public Boolean TerminalRiskManagement
        {
            get { return (tlv.value[0] & 0x08) != 0x00; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="ApplicationInterchangeProfile"/> instance.
        /// </summary>
        public ApplicationInterchangeProfile()
        {
            tlv = new TLVData { tag = 0x82, value = new Byte[2] };
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationInterchangeProfile"/> instance.
        /// </summary>
        /// <param name="aipBytes">Byte array of 2 bytes containing the value.</param>
        public ApplicationInterchangeProfile(Byte[] aipBytes)
            : this()
        {
            tlv.value[0] = aipBytes[0];
            tlv.value[1] = aipBytes[1];
        }

        /// <summary>
        /// Initializes a new <see cref="ApplicationInterchangeProfile"/> instance.
        /// </summary>
        /// <param name="hi">First byte of AIP (left).</param>
        /// <param name="lo">Second byte of AIP (right).</param>
        public ApplicationInterchangeProfile(Byte hi, Byte lo)
            : this()
        {
            tlv.value[0] = hi;
            tlv.value[1] = lo;
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = "";
            s += (Sda ? "SDA " : "");
            s += (Dda ? "DDA " : "");
            s += (Cda ? "CDA " : "");
            s += (IssuerAuthentication ? "| Issuer Authentication " : "");
            s += (CardholderVerification ? "| Cardholder Verification " : "");
            s += (TerminalRiskManagement ? "| Terminal Risk Management" : "");
            return s;
        }

        #endregion
    }
}
