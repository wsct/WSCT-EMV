using System;
using WSCT.Helpers;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Represents a plaintext offline PINBlock.
    /// <para>PIN block format: <c>C N   P P   P P   P/F P/F   P/F P/F   P/F P/F   P/F P/F   F F</c></para>.
    /// </summary>
    public class PlaintextPINBlock : PINBlock
    {
        #region >> Constants

        const Byte ControlField = 0x2;
        const Byte Filler = 0xF;

        #endregion

        #region >> Fields

        Byte[] _pinBlock;

        #endregion

        #region >> AbstractPINBlock

        /// <inheritdoc />
        public override Byte ClearPINLength
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
        public override Byte[] ClearPIN
        {
            get
            {
                var bcdPIN = new Byte[ClearPINLength];
                Array.Copy(_pinBlock, 1, bcdPIN, 0, ClearPINLength);
                return bcdPIN.fromBCD(ClearPINLength);
            }
            set
            {
                ClearPINLength = (Byte)value.Length;
                value.toBCD(Filler).CopyTo(_pinBlock, 1);
            }
        }

        /// <inheritdoc />
        public override Byte[] PinBlock
        {
            get { return _pinBlock; }
            set { _pinBlock = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new <see cref="PlaintextPINBlock"/> instance.
        /// </summary>
        public PlaintextPINBlock()
        {
            _pinBlock = new Byte[8];
            _pinBlock[0] = (ControlField << 4) + 0x0;
            _pinBlock[1] = (Filler << 4) + Filler;
            _pinBlock[2] = (Filler << 4) + Filler;
            _pinBlock[3] = (Filler << 4) + Filler;
            _pinBlock[4] = (Filler << 4) + Filler;
            _pinBlock[5] = (Filler << 4) + Filler;
            _pinBlock[6] = (Filler << 4) + Filler;
            _pinBlock[7] = (Filler << 4) + Filler;
        }

    }
}
