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
            Selection = SelectionMode.SelectDFName;
            Occurence = FileOccurrence.FirstOrOnly;
            Information = FileControlInformation.ReturnFci;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVSelectByNameCommand"/> instance.
        /// </summary>
        /// <param name="name">DF Name or AID.</param>
        public EMVSelectByNameCommand(byte[] name) :
            this()
        {
            Udc = name;
        }

        /// <summary>
        /// Initializes a new <see cref="EMVSelectByNameCommand"/> instance.
        /// </summary>
        /// <param name="name">DF Name or AID.</param>
        /// <param name="le">Length of FCI.</param>
        public EMVSelectByNameCommand(byte[] name, uint le) :
            this(name)
        {
            Le = le;
        }

        #endregion
    }
}