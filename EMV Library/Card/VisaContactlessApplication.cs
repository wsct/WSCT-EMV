using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WSCT.Core;
using WSCT.EMV.Objects;
using WSCT.EMV.Security;
using WSCT.Helpers.BasicEncodingRules;

namespace WSCT.EMV.Card
{
    public class VisaContactlessApplication : EMVApplication
    {

        #region >> Fields

        /// <summary>
        /// Dynamic Data Authentication object
        /// </summary>
        DynamicDataAuthentication _dda;

        /// <summary>
        /// Length of Signed Dynamic Application Data
        /// </summary>
        int _nic;

        #endregion

        #region >> Constructors

        public VisaContactlessApplication(ICardChannel cardChannel)
            : base(cardChannel)
        {

        }

        #endregion

        #region >> Properties

        public DynamicDataAuthentication dda
        {
            get
            {
                if (_dda == null && iccPublicKeyCertificate != null)
                {
                    Byte[] signature = null;
                    signature = tlvProcessingOptions.getTag(0x9F4B).value;
                    _nic = signature.Length;
                    
                    _dda = new DynamicDataAuthentication();
                    _dda.recoverFromSignature(signature, iccPublicKey);
                }
                return _dda;
            }
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Verifies the Fast DDA Signature
        /// </summary>
        /// <param name="fDDAelements"></param>
        /// <returns></returns>
        public Boolean verifiesFastDDA()
        {
            Cryptography _cryptography = new Cryptography();

            // Check that READ APPLICATION DATA has been performed
            if (tlvRecords.Count == 0) readApplicationData();

            if (dda != null && tlvDataRecords.hasTag(0x9F69))
            {
                byte fDDAversion = tlvDataRecords.getTag(0x9F69).value[0];

                switch (fDDAversion)
                {
                    case 0x00:
                        if (tlvDataTerminalData.hasTag(0x9F37)      // Terminal Unpredictable Number
                            && tlvDataRecords.hasTag(0x9F36))       // ATC
                        {
                            uint k = (uint)_nic - _dda.iccDynamicDataLength - 25;
                            uint offset = 0;
                            uint length9F37 = tlvDataTerminalData.getTag(0x9F37).length;
                            uint length9F36 = tlvDataRecords.getTag(0x9F36).length;

                            byte[] data = new byte[3 + k + length9F37];
                            data[0] = 0x05; // Signed Data Format
                            data[1] = 0x01; // Hash Algorithm Indicator
                            data[2] = _dda.iccDynamicDataLength; // ICC Dynamic Data Length
                            data[3] = (byte)length9F36;
                            offset = 4;

                            Array.Copy(tlvDataRecords.getTag(0x9F36).value, 0, data, offset, length9F36); // ATC
                            offset += length9F36;

                            byte[] padding = new byte[k];
                            for (int i = 0; i < padding.Length; i++) padding[i] = 0xBB;
                            Array.Copy(padding, 0, data, offset, k); // Pad Pattern
                            offset += k;

                            Array.Copy(tlvDataTerminalData.getTag(0x9F37).value, 0, data, offset, length9F37);
                            offset += length9F37;

                            byte[] hash = _cryptography.computeHash(data);
                            if (hash.SequenceEqual(_dda.hashResult))
                                return true;
                        }
                        else return false;
                        break;
                    
                    case 0x01:
                        if (tlvDataTerminalData.hasTag(0x9F37)      // Terminal Unpredictable Number
                            && tlvDataTerminalData.hasTag(0x9F02)   // Amount Authorised
                            && tlvDataTerminalData.hasTag(0x5F2A)   // Transaction Currency Code
                            && tlvProcessingOptions.hasTag(0x9F36)  // ATC
                            && tlvDataRecords.hasTag(0x9F69))       // Card Authentication Related Data
                        {
                            uint offset = 0;
                            uint iccDynamicDataLength = _dda.iccDynamicDataLength;
                            uint k = (uint)_nic - iccDynamicDataLength - 25;
                            uint length9F37 = tlvDataTerminalData.getTag(0x9F37).length;
                            uint length9F36 = tlvProcessingOptions.getTag(0x9F36).length;
                            uint length9F02 = tlvDataTerminalData.getTag(0x9F02).length;
                            uint length5F2A = tlvDataTerminalData.getTag(0x5F2A).length;
                            uint length9F69 = tlvDataRecords.getTag(0x9F69).length;

                            byte[] data = new byte[3 + 1 + length9F36 + k + length9F37 + length9F02 + length5F2A + length9F69];
                            data[0] = 0x05; // Signed Data Format
                            data[1] = 0x01; // Hash Algorithm Indicator
                            data[2] = (byte)iccDynamicDataLength; // ICC Dynamic Data Length
                            data[3] = (byte)length9F36;
                            offset = 4;

                            Array.Copy(tlvProcessingOptions.getTag(0x9F36).value, 0, data, offset, length9F36); // ATC as ICC Dynamic Data
                            offset += length9F36;

                            byte[] padding = new byte[k];
                            for (int i = 0; i < padding.Length; i++) padding[i] = 0xBB;
                            Array.Copy(padding, 0, data, offset, k); // Pad Pattern
                            offset += k;

                            Array.Copy(tlvDataTerminalData.getTag(0x9F37).value, 0, data, offset, length9F37);
                            offset += length9F37;

                            Array.Copy(tlvDataTerminalData.getTag(0x9F02).value, 0, data, offset, length9F02);
                            offset += length9F02;

                            Array.Copy(tlvDataTerminalData.getTag(0x5F2A).value, 0, data, offset, length5F2A);
                            offset += length5F2A;

                            Array.Copy(tlvDataRecords.getTag(0x9F69).value, 0, data, offset, length9F69);
                            offset += length9F69;

                            byte[] hash = _cryptography.computeHash(data);
                            if (hash.SequenceEqual(_dda.hashResult))
                                return true;
                        }
                        else return false;
                        break;
                    default:
                        return false;
                }
            }
            return false;
        }

        #endregion

    }
}
