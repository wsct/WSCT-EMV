﻿using System;
using System.Text;
using WSCT.Helpers;

namespace WSCT.EMV.Security
{
    /// <summary>
    /// Represents an EMV Data Authentication (Data Format: 05).
    /// </summary>
    /// <remarks>Format used for DDA and CDA.</remarks>
    public abstract class AbstractDataAuthenticationFormat05 : AbstractSignatureContainer
    {
        #region >> Properties

        /// <summary>
        /// ICC Dynamic Data Length: Identifies the length of the ICC Dynamic Data in bytes.
        /// </summary>
        public byte IccDynamicDataLength { get; set; }

        /// <summary>
        /// ICC Dynamic Data (LDD):  Dynamic data generated by and/or stored in the ICC.
        /// </summary>
        public byte[] IccDynamicData { get; set; }

        /// <summary>
        /// Pad Pattern (NIC - LDD - 25): (NIC - LDD - 25) padding bytes of value 'BB'.
        /// </summary>
        public byte[] PadPattern { get; set; }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="AbstractDataAuthenticationFormat05"/> instance.
        /// </summary>
        protected AbstractDataAuthenticationFormat05()
            : base(2)
        {
        }

        #endregion

        #region >> Object

        /// <inheritdoc />
        public override string ToString()
        {
            var s = new StringBuilder();
            s.AppendFormat("Header:[{0:X2}] ", DataHeader);
            s.AppendFormat("Format:[{0:X2}] ", DataFormat);
            s.AppendFormat("Hash Algorithm:[{0:X2}] ", HashAlgorithmIndicator);
            s.AppendFormat("ICC Dynamic Data Length:[{0:X2}] ", IccDynamicDataLength);
            s.AppendFormat("ICC Dynamic Data:[{0}] ", IccDynamicData.ToHexa('\0'));
            s.AppendFormat("Pad:[{0}] ", PadPattern.ToHexa('\0'));
            s.AppendFormat("Hash:[{0}] ", HashResult.ToHexa('\0'));
            s.AppendFormat("Trailer:[{0:X2}] ", DataTrailer);

            return s.ToString();
        }

        #endregion

        #region >> AbstractSignatureContainer

        /// <param name="privateKeyLength"></param>
        /// <inheritdoc />
        protected override byte[] GetDataToSign(int privateKeyLength)
        {
            // TODO : Build data to sign
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void OnRecoverFromSignature()
        {
            IccDynamicDataLength = Recovered[3];

            IccDynamicData = new byte[IccDynamicDataLength];
            Array.Copy(Recovered, 4, IccDynamicData, 0, IccDynamicDataLength);

            PadPattern = new byte[KeyLength - IccDynamicDataLength - 25 - 1];
            Array.Copy(Recovered, 5 + IccDynamicDataLength, PadPattern, 0, KeyLength - IccDynamicDataLength - 25 - 1);
        }

        #endregion
    }
}