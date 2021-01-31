using WSCT.Helpers;
using WSCT.ISO7816;

namespace WSCT.EMV.Commands
{
    /// <summary>
    /// EMV STORE DATA command.
    /// </summary>
    public class EMVStoreDataCommand : CommandAPDU
    {
        [System.Flags]
        public enum Encryption : byte
        {
            AllDGIEncrypted = 0b_0110_0000,
            NoDGIEncrypted = 0b_0000_0000,
            ApplicationDependant = 0b_0010_0000,
            ReservedForFutureUse = 0b_0100_0000
        }

        public byte SequenceNumber
        {
            get => P2;
            set => P2 = value;
        }

        public bool IsLast
        {
            get => (P1 & 0b_1000_0000) != 0;
            set => P1 = (byte)(value ? P1 | 0b_1000_0000 : P1 & 0b_0111_111);
        }

        public Encryption EncryptionIndicator
        {
            get => (Encryption)(P1 & 0b_0110_0000);
            set => P1 = (byte)((P1 & 0b_1001_1111) | (byte)EncryptionIndicator);
        }

        public string DGIs
        {
            get => Udc.ToHexa();
            set => Udc = value.FromHexa();
        }

        #region >> Constructors

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public EMVStoreDataCommand()
        {
            base.Cla = 0x80;
            base.Ins = 0xE2;
        }

        /// <summary>
        /// Initializes and parameters a new instance.
        /// </summary>
        /// <param name="sequenceNumber"></param>
        /// <param name="isLast"></param>
        /// <param name="encryptionIndicator"></param>
        /// <param name="dgi"></param>
        public EMVStoreDataCommand(byte sequenceNumber, bool isLast, Encryption encryptionIndicator, string dgi)
            : this()
        {
            SequenceNumber = sequenceNumber;
            IsLast = isLast;
            EncryptionIndicator = encryptionIndicator;
            DGIs = dgi;
        }

        #endregion
    }
}
