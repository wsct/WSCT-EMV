using System;
using System.Collections.Generic;
using System.Text;

using WSCT.Helpers;

namespace WSCT.EMV.Card
{
    /// <summary>
    /// Represents an abstract PIN block
    /// </summary>
    public abstract class PINBlock
    {
        #region >> Constants

        /// <summary>
        /// Default value for undefined clean PIN
        /// </summary>
        public /*const*/ Byte[] NullClearPIN = new Byte[4] { 0, 0, 0, 0 };

        #endregion

        #region >> Properties

        /// <summary>
        /// Clear PIN length accessor
        /// </summary>
        public abstract Byte clearPINLength
        {
            get;
            set;
        }

        /// <summary>
        /// Clear PIN accessor
        /// </summary>
        public abstract Byte[] clearPIN
        {
            get;
            set;
        }

        /// <summary>
        /// Accessor to the PIN block built
        /// </summary>
        public abstract Byte[] pinBlock
        {
            get;
            set;
        }

        #endregion

        #region >> Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PINBlock()
        {
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return pinBlock.toHexa();
        }
    }
}
