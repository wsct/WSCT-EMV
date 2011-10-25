using System;
using System.Collections.Generic;
using System.Text;

using WSCT.EMV;
using WSCT.Helpers;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Objects
{
    /// <summary>
    /// Represents the Application Interchange Profile of an EMV application
    /// </summary>
    public class ApplicationInterchangeProfile : AbstractTLVObject
    {
        #region >> Properties

        /// <summary>
        /// Informs if CDA is supported
        /// </summary>
        public Boolean cda
        {
            get { return (tlv.value[0] & 0x01) != 0x00; }
        }

        /// <summary>
        /// Informs if DDA is supported
        /// </summary>
        public Boolean dda
        {
            get { return (tlv.value[0] & 0x20) != 0x00; }
        }

        /// <summary>
        /// Informs if SDA is supported
        /// </summary>
        public Boolean sda
        {
            get { return (tlv.value[0] & 0x40) != 0x00; }
        }

        /// <summary>
        /// Informs if Cardholder verification is supported
        /// </summary>
        public Boolean cardholderVerification
        {
            get { return (tlv.value[0] & 0x10) != 0x00; }
        }

        /// <summary>
        /// Informs if Issuer authentication is supported
        /// </summary>
        public Boolean issuerAuthentication
        {
            get { return (tlv.value[0] & 0x04) != 0x00; }
        }

        /// <summary>
        /// Informs if Terminal Risk Management is to be performed
        /// </summary>
        public Boolean terminalRiskManagement
        {
            get { return (tlv.value[0] & 0x08) != 0x00; }
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public ApplicationInterchangeProfile()
            : base()
        {
            tlv = new WSCT.Helpers.BasicEncodingRules.TLVData();
            tlv.value = new Byte[2];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aipBytes">Byte array of 2 bytes containing the value</param>
        public ApplicationInterchangeProfile(Byte[] aipBytes)
            : this()
        {
            tlv.value[0] = aipBytes[0];
            tlv.value[1] = aipBytes[1];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hi">First byte of AIP (left)</param>
        /// <param name="lo">Second byte of AIP (right)</param>
        public ApplicationInterchangeProfile(Byte hi, Byte lo)
            : this()
        {
            tlv.value[0] = hi;
            tlv.value[1] = lo;
        }

        #endregion

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String s = "";
            s += (sda ? "SDA " : "");
            s += (dda ? "DDA " : "");
            s += (cda ? "CDA " : "");
            s += (issuerAuthentication ? "| Issuer Authentication " : "");
            s += (cardholderVerification ? "| Cardholder Verification " : "");
            s += (terminalRiskManagement ? "| Terminal Risk Management" : "");
            return s;
        }

    }
}
