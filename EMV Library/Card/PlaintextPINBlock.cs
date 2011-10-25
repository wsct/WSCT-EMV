using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Represents a plaintext offline PINBlock
    /// <para>PIN block format: <c>C N   P P   P P   P/F P/F   P/F P/F   P/F P/F   P/F P/F   F F</c></para>
    /// </summary>
    public class PlaintextPINBlock : PINBlock
    {
        #region >> Constants

        const Byte CONTROL_FIELD = 0x2;
        const Byte FILLER = 0xF;

        #endregion

        #region >> Fields

        Byte[] _pinBlock;

        #endregion

        #region >> AbstractPINBlock

        /// <inheritdoc />
        public override Byte clearPINLength
        {
            get { return (Byte)(_pinBlock[0] & 0x0F); }
            set
            {
                if (value < 4 || value > 12)
                {
                    throw new Exception(String.Format("PlainTextPINBlock: PIN length must be in range [4-12] (found {0})", value));
                }
                _pinBlock[0] = (Byte)((_pinBlock[0] & 0xF0) + value);
            }
        }

        /// <inheritdoc />
        public override Byte[] clearPIN
        {
            get
            {
                Byte[] bcdPIN = new Byte[clearPINLength];
                Array.Copy(_pinBlock, 1, bcdPIN, 0, clearPINLength);
                return bcdPIN.fromBCD(clearPINLength);
            }
            set
            {
                clearPINLength = (Byte)value.Length;
                value.toBCD(FILLER).CopyTo(_pinBlock, 1);
            }
        }

        /// <inheritdoc />
        public override Byte[] pinBlock
        {
            get { return _pinBlock; }
            set { _pinBlock = value; }
        }

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlaintextPINBlock()
            : base()
        {
            _pinBlock = new Byte[8];
            _pinBlock[0] = (Byte)((CONTROL_FIELD << 4) + 0x0);
            _pinBlock[1] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[2] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[3] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[4] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[5] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[6] = (Byte)((FILLER << 4) + FILLER);
            _pinBlock[7] = (Byte)((FILLER << 4) + FILLER);
        }

    }
}
