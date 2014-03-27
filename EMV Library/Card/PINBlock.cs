using WSCT.Helpers;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Represents an abstract PIN block.
    /// </summary>
    public abstract class PINBlock
    {
        #region >> Constants

        /// <summary>
        /// Default value for undefined clean PIN.
        /// </summary>
        public byte[] NullClearPIN = { 0, 0, 0, 0 };

        #endregion

        #region >> Properties

        /// <summary>
        /// Clear PIN length accessor.
        /// </summary>
        public abstract byte ClearPINLength { get; set; }

        /// <summary>
        /// Clear PIN accessor.
        /// </summary>
        public abstract byte[] ClearPIN { get; set; }

        /// <summary>
        /// Accessor to the PIN block built.
        /// </summary>
        public abstract byte[] PinBlock { get; set; }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return PinBlock.ToHexa();
        }
    }
}