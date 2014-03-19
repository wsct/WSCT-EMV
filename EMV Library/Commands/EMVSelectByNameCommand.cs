using System;
using WSCT.ISO7816.Commands;

namespace WSCT.EMV.Commands
{
    /// <summary>
    /// EMV Select by DF Name C-APDU.
    /// </summary>
    public class EMVSelectByNameCommand : SelectCommand
    {
        #region >> Constructors

        /// <summary>
        /// Initializes a new <see cref="EMVSelectByNameCommand"/> instance.
        /// </summary>
        public EMVSelectByNameCommand()
        {
            selectionMode = SelectionMode.SELECT_DF_NAME;
            fileOccurence = FileOccurrence.FIRST_OR_ONLY;
            fileControlInformation = FileControlInformation.RETURN_FCI;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVSelectByNameCommand"/> instance.
        /// </summary>
        /// <param name="name">DF Name or AID.</param>
        public EMVSelectByNameCommand(Byte[] name) :
            this()
        {
            udc = name;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVSelectByNameCommand"/> instance.
        /// </summary>
        /// <param name="name">DF Name or AID.</param>
        /// <param name="le">Length of FCI.</param>
        public EMVSelectByNameCommand(Byte[] name, uint le) :
            this(name)
        {
            this.le = le;
        }

        #endregion
    }
}
