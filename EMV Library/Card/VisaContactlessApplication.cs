using System;
using System.Linq;
using WSCT.Core;
using WSCT.EMV.Security;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Specialized <see cref="EMVApplication"/> for Visa contactless application.
    /// </summary>
    public class VisaContactlessApplication : EMVApplication
    {
        #region >> Fields

        /// <summary>
        /// Length of Signed Dynamic Application Data.
        /// </summary>
        int _nic;

        #endregion

        #region >> Constructors

        /// <summary>
        /// Creates a new <see cref="VisaContactlessApplication"/> instance.
        /// </summary>
        /// <param name="cardChannel"></param>
        public VisaContactlessApplication(ICardChannel cardChannel)
            : base(cardChannel)
        {
        }

        #endregion

        #region >> Properties

        /// <inheritdoc />
        public override DynamicDataAuthentication Dda
        {
            get
            {
                if (_dda == null && IccPublicKeyCertificate != null)
                {
                    var signature = TlvProcessingOptions.getTag(0x9F4B).value;
                    _nic = signature.Length;

                    _dda = new DynamicDataAuthentication();
                    _dda.recoverFromSignature(signature, IccPublicKey);
                }
                return _dda;
            }
        }

        #endregion

        #region >> Methods

        /// <summary>
        /// Verifies the Fast DDA Signature.
        /// </summary>
        /// <returns></returns>
        public Boolean VerifiesFastDda()
        {
            var cryptography = new Cryptography();

            // Check that READ APPLICATION DATA has been performed
            if (TlvRecords.Count == 0) ReadApplicationData();

            if (Dda != null && TlvDataRecords.hasTag(0x9F69))
            {
                var fDdAversion = TlvDataRecords.getTag(0x9F69).value[0];

                switch (fDdAversion)
                {
                    case 0x00:
                        if (TlvDataTerminalData.hasTag(0x9F37)      // Terminal Unpredictable Number
                            && TlvDataRecords.hasTag(0x9F36))       // ATC
                        {
                            var k = (uint)_nic - _dda.iccDynamicDataLength - 25;
                            var length9F37 = TlvDataTerminalData.getTag(0x9F37).length;
                            var length9F36 = TlvDataRecords.getTag(0x9F36).length;

                            var data = new byte[3 + k + length9F37];
                            data[0] = 0x05; // Signed Data Format
                            data[1] = 0x01; // Hash Algorithm Indicator
                            data[2] = _dda.iccDynamicDataLength; // ICC Dynamic Data Length
                            data[3] = (byte)length9F36;
                            uint offset = 4;

                            Array.Copy(TlvDataRecords.getTag(0x9F36).value, 0, data, offset, length9F36); // ATC
                            offset += length9F36;

                            var padding = new byte[k];
                            for (var i = 0; i < padding.Length; i++) padding[i] = 0xBB;
                            Array.Copy(padding, 0, data, offset, k); // Pad Pattern
                            offset += k;

                            Array.Copy(TlvDataTerminalData.getTag(0x9F37).value, 0, data, offset, length9F37);
                            // offset += length9F37;

                            var hash = cryptography.ComputeHash(data);
                            if (hash.SequenceEqual(_dda.hashResult))
                                return true;
                        }
                        else return false;
                        break;

                    case 0x01:
                        if (TlvDataTerminalData.hasTag(0x9F37)      // Terminal Unpredictable Number
                            && TlvDataTerminalData.hasTag(0x9F02)   // Amount Authorised
                            && TlvDataTerminalData.hasTag(0x5F2A)   // Transaction Currency Code
                            && TlvProcessingOptions.hasTag(0x9F36)  // ATC
                            && TlvDataRecords.hasTag(0x9F69))       // Card Authentication Related Data
                        {
                            uint iccDynamicDataLength = _dda.iccDynamicDataLength;
                            var k = (uint)_nic - iccDynamicDataLength - 25;
                            var length9F37 = TlvDataTerminalData.getTag(0x9F37).length;
                            var length9F36 = TlvProcessingOptions.getTag(0x9F36).length;
                            var length9F02 = TlvDataTerminalData.getTag(0x9F02).length;
                            var length5F2A = TlvDataTerminalData.getTag(0x5F2A).length;
                            var length9F69 = TlvDataRecords.getTag(0x9F69).length;

                            var data = new byte[3 + 1 + length9F36 + k + length9F37 + length9F02 + length5F2A + length9F69];
                            data[0] = 0x05; // Signed Data Format
                            data[1] = 0x01; // Hash Algorithm Indicator
                            data[2] = (byte)iccDynamicDataLength; // ICC Dynamic Data Length
                            data[3] = (byte)length9F36;
                            uint offset = 4;

                            Array.Copy(TlvProcessingOptions.getTag(0x9F36).value, 0, data, offset, length9F36); // ATC as ICC Dynamic Data
                            offset += length9F36;

                            var padding = new byte[k];
                            for (var i = 0; i < padding.Length; i++) padding[i] = 0xBB;
                            Array.Copy(padding, 0, data, offset, k); // Pad Pattern
                            offset += k;

                            Array.Copy(TlvDataTerminalData.getTag(0x9F37).value, 0, data, offset, length9F37);
                            offset += length9F37;

                            Array.Copy(TlvDataTerminalData.getTag(0x9F02).value, 0, data, offset, length9F02);
                            offset += length9F02;

                            Array.Copy(TlvDataTerminalData.getTag(0x5F2A).value, 0, data, offset, length5F2A);
                            offset += length5F2A;

                            Array.Copy(TlvDataRecords.getTag(0x9F69).value, 0, data, offset, length9F69);
                            // offset += length9F69;

                            var hash = cryptography.ComputeHash(data);
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
